using Auluxa.Repositories;
using Auluxa.Repositories.Contexts;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Auluxa.WebApi
{
    /// <summary>
    /// WebApi configuration class
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Load the httpConfiguration
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            //config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Use camel case for JSON data and remove XML support
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(
                new StringEnumConverter { CamelCaseText = true });
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Set dependency injection
            SetAutofacContainer(config);

            // Web API routes
            config.MapHttpAttributeRoutes();

            // Ensure initializations of config and starts to listen.
            config.EnsureInitialized();

            // Enables CORS
            config.EnableCors();

            // Ensure initialization is correct
            config.EnsureInitialized();
        }

        /// <summary>
        /// Set dependency injection using the IoC pattern using Autofac
        /// </summary>
        /// <param name="config">The HttpConfiguration instance to be injected.</param>
        private static void SetAutofacContainer(HttpConfiguration config)
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
