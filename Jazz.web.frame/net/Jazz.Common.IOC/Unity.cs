using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;

namespace Jazz.Common.IOC
{
    public class Unity
    {
        private IUnityContainer container;

        private string configFile;

         public Unity(string configPath)
         {
             this.configFile = configPath;


             var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = configFile };
             Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
             UnityConfigurationSection section = (UnityConfigurationSection)configuration.GetSection("unity");
             container = new UnityContainer().LoadConfiguration(section, "MyContainer");
         }

         public void RegisterInstance<T>(T target)
         {
             container.RegisterInstance<T>(target);
         }

         public object GetService(Type serviceType)
         {
            
             return container.Resolve(serviceType);
         }
         public T GetService<T>()
         {
             return container.Resolve<T>();
         }
         public T GetService<T>(params ParameterOverride[] obj)
         {
             return container.Resolve<T>(obj);
         }
         public T GetService<T>(string name, params ParameterOverride[] obj)
         {
             return container.Resolve<T>(name, obj);
         }
    }
}
