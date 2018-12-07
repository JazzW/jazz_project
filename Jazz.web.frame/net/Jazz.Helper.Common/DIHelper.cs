using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Jazz.Helper.Common
{
    public class DIHelper
    {
        
        public IServiceCollection Service{get;set;}

        public DIHelper()
        {
            Service = new ServiceCollection();
        }

        public IServiceCollection AddTransient<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory)
            where TService : class
            where TImplementation : class, TService
        {
            return Service.AddTransient<TService, TImplementation>(implementationFactory);
        }

        public IServiceCollection AddSingleton<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory)
            where TService : class
            where TImplementation : class, TService
        {
            return Service.AddSingleton<TService, TImplementation>(implementationFactory);
        }

        public IServiceCollection AddScoped<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory)
            where TService : class
            where TImplementation : class, TService
        {
            return Service.AddScoped<TService, TImplementation>(implementationFactory);
        }

        public T getService<T>()
        {
         
            IServiceProvider Provider = Service.BuildServiceProvider();
            return Provider.GetService<T>();
        }


        public static T getService<T>(IServiceCollection serice)
        {
            IServiceProvider Provider = serice.BuildServiceProvider();
            return Provider.GetService<T>();
        }

    }
}
