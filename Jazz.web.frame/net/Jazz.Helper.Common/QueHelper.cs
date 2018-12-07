using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Jazz.Helper.Common
{
    class QueHelper
    {
        private Queue _que;

        private List<Job> _flist = new List<Job>();

        private bool _run=false ;

        public void Run()
        {
            this._run = true;

            while (this._run)
            {
                if (_que.Count == 0) { System.Threading.Thread.Sleep(1000); continue; }
                Job job =_que.Dequeue() as Job;
                job.Run();
                _flist.Add(job);
            }
        }

        public void Stop()
        {
            this._run=false;
        }

        public Guid Add(Job job)
        {
            lock (_que)
            {
                job.JobKey = Guid.NewGuid();
                _que.Enqueue(job);
                return job.JobKey;
            }
        }

        public Task<object> GetResult(Guid guid)
        {
            return
            Task.Factory.StartNew<object>(() => 
            {
                int i = 0;
                while (_run)
                {
                    Job job = this.GetJob(guid);
                    if (job == null)
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                    else
                    {
                        return job.JobResult;
                    }
                    i++;
                }
                return null;
            });
        }

        public Job GetJob(Guid guid)
        {
            try
            {
                return _flist.Where(e => e.JobKey == guid).First();
            }
            catch
            {
                return null;
            }
        }
    }

    public class Job:Object
    {
        public Guid JobKey { get; set; }

        public Func<Hashtable, object> JobBody { get; set; }

        public Hashtable JobPars { get; set; }

        public object JobResult { get; set; }

        public Exception JobEx { get; set; }

        public int JobState { get; set; }

        public int Run()
        {
            try
            {
                this.JobState = 0;
                this.JobResult = this.JobBody.Invoke(this.JobPars);
                this.JobState = 1;
            }
            catch (Exception ex)
            {
                this.JobState = -1;
                this.JobEx = ex;
            }
            return this.JobState;
        }
    }
}
