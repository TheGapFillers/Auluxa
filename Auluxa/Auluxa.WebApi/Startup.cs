using System.Reflection;
using System.Web.Http;
using Auluxa.Repositories;
using Auluxa.Repositories.Contexts;
using Autofac;
using Autofac.Integration.WebApi;
using Newtonsoft.Json.Serialization;
using Owin;

namespace Auluxa.WebApi
{
    /// <summary>
    /// Class being called by the host on start of the application.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Method being called by the Owin layer to configure the application
        /// </summary>
        /// <param name="appBuilder">Injected appBuilder from the host.</param>
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();

            // Use camel case for JSON data and remove XML support
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Set dependency injection
            SetAutofacContainer(config);

            // Web API routes
            config.MapHttpAttributeRoutes();

            // Ensure initializations of config and starts to listen.
            config.EnsureInitialized();
            appBuilder.UseWebApi(config);
        }

        /// <summary>
        /// Set dependency injection using the IoC pattern using Autofac
        /// </summary>
        /// <param name="config">The HttpConfiguration instance to be injected.</param>
        private void SetAutofacContainer(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();

            // Register all the ApiController belonging to this assembly.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Register all the types to be abstracted / white labeled
            // Scene repository
            builder.RegisterType<ApplicationDbContext>().As<IApplicationDbContext>();
            builder.RegisterType<EfApplicationRepository>().As<IApplicationRepository>().PropertiesAutowired();
            
            // Build the container and set the dependency resolver of the config.
            IContainer container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
