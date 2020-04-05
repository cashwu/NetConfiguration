using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.Extensions.Configuration;
using testNetJsonConfig.Models;

namespace testNetJsonConfig
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var configurationBuilder = new ConfigurationBuilder()
                                       .SetBasePath(Directory.GetCurrentDirectory())
                                       .AddJsonFile("config.json");

            var config = configurationBuilder.Build();

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterControllers(Assembly.GetExecutingAssembly()); 
            containerBuilder.Register(context => config).As<IConfiguration>().InstancePerLifetimeScope();

            containerBuilder.Register(a =>
            {
                var redisConfig = new RedisConfig();
                a.Resolve<IConfiguration>().GetSection("Redis").Bind(redisConfig);
                return redisConfig;
            }).InstancePerLifetimeScope();
            
            var container = containerBuilder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}