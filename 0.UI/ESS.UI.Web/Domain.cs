#region

using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Http;
using ESS.Framework.Common.Autofac;
using ESS.Framework.Common.JsonNet;
using ESS.Framework.Common.Log4Net;
using ESS.Framework.CQRS;
using ESS.Framework.CQRS.Configurations;
using ESS.Framework.CQRS.Event;
using ESS.Framework.Data.InMemory;
using ESS.Framework.Data.Redis;
using ESS.Framework.Licensing.OAuth;
using ESS.Framework.UI.Configurations;
using Owin;
using Configuration = ESS.Framework.Common.Configurations.Configuration;

#endregion

namespace ESS.UI.Web
{
    public static class Domain
    {
        public static void Setup(IAppBuilder app, HttpConfiguration config)
        {
            var connString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            IEventStore es =  new SqlEventStore(connString);

            //IEventStore es = new RedisEventStore("127.0.0.1", "6379");

            var assemblies = BuildManager.GetReferencedAssemblies()
                .Cast<Assembly>()
                .Where(c => c.FullName.Contains("ESS"))
                .ToArray();

            Configuration.Create()
                .UseAutofac()
                .RegisterCommonComponents()
                .UseLog4Net()
                .UseJsonNet()
                .UseRedisRepository()
                .RegisterBusinessComponents(assemblies)
                .InitializeCQRSAssemblies(es, assemblies)
                .RegisterController(config, assemblies)
                .UseOAuth(app, config)
                .UseWebApi(app, config);

            //内存模式测试用
            //new Api.Foundation.InitData().Init();
            //new Api.Common.InitData().Init();
            
        }
    }
}