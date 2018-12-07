using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using System.Data.Common;
using System.Data;
using System.Text.RegularExpressions;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Reflection;
using System.Data.SqlClient;
using System.Configuration;
using Jazz.EFframe.IMap;

namespace Jazz.EF.Frame.IModel
{
     public class MyDbContext : DbContext
     {
         public static  Type AssemblyName;

         public static Assembly asm;

         private static SqlConnection GetEFConnctionString(string connString)
         {
             var obj = System.Configuration.ConfigurationManager.ConnectionStrings[connString];
             SqlConnection con;
             if (obj != null)
             {
                 con = new SqlConnection(obj.ConnectionString);
             }
             else
             {
                 con = new SqlConnection(connString);
             }

             return con;

         }

         public MyDbContext(string connString)
            : base(GetEFConnctionString(connString),true)
        {
            this.Configuration.AutoDetectChangesEnabled = false;
            this.Configuration.ValidateOnSaveEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

         protected override void OnModelCreating(DbModelBuilder modelBuilder)
         {
             //string assembleFileName = Assembly.GetExecutingAssembly().CodeBase.Replace("Jazz").Replace("file:///", "");
             if(asm==null)
                asm = Assembly.GetAssembly(AssemblyName);
             var typesToRegister = asm.GetTypes()
                 .Where(type => !String.IsNullOrEmpty(type.Namespace))
                .Where(type => type.BaseType != null && type.BaseType.BaseType != null && type.BaseType.BaseType.IsGenericType && type.BaseType.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));

             //.Where(type => !String.IsNullOrEmpty(type.Namespace))
             //.Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(BaseDomainMapping<>));
             foreach (var type in typesToRegister)
             {
                 dynamic configurationInstance = Activator.CreateInstance(type);
                 modelBuilder.Configurations.Add(configurationInstance);
             }
             base.OnModelCreating(modelBuilder);
         }
     }
}
