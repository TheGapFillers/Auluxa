using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ExceptionHandling;
using Auluxa.WebApp.ApplicationContext;
using Auluxa.WebApp.Auth;
using Auluxa.WebApp.Devices.Repositories;
using Auluxa.WebApp.ErrorHandling;
using Auluxa.WebApp.Kranium.Repositories;
using Auluxa.WebApp.Scenes.Repositories;
using Auluxa.WebApp.Subscription;
using Auluxa.WebApp.UserSettings.Repositories;
using Auluxa.WebApp.Zones.Repositories;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Swashbuckle.Application;

namespace Auluxa.WebApp
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
            //config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Use camel case for JSON data and remove XML support
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(
                new StringEnumConverter { CamelCaseText = true });
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Enables CORS
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            // Set dependency injection
            SetAutofacContainer(config);

            // Exception handler
            config.Services.Replace(typeof(IExceptionHandler), new AuluxaExceptionHandler());

            // Web API routes
            config.MapHttpAttributeRoutes();

            // Swagger
            config.EnableSwagger(c =>
            {
                // Manage title and versioning
                c.SingleApiVersion("v1", ConfigurationManager.AppSettings["ServiceName"]);

                // Automatically gets the generated XML doc of the controllers and inject them in swagger
                string baseDirectory = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;

                // Include XmlComments
                string commentFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
                c.IncludeXmlComments(Path.Combine(baseDirectory, commentFileName));
            })
            .EnableSwaggerUi("help/{*assetPath}", c =>
            {
                // Inject auth URL as 'realm'. Could do better
                c.EnableOAuth2Support(
                    "web",
                    "nosecret",
                    ConfigurationManager.AppSettings["auluxa-auth:Url"],
                    "auluxa-auth");

                // Deactivate validator
                c.DisableValidator();

                // Inject login form javascript
                c.InjectJavaScript(Assembly.GetExecutingAssembly(), "Auluxa.WebApp.onComplete.js");
            });

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
            // auth
            builder.RegisterType<AuthDbContext>().As<IdentityDbContext<AuthUser>>();
            builder.RegisterType<AuthUserManager>().InstancePerRequest();
            builder.RegisterType<AuthSignInManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<AuthUserStore>().As<IUserStore<AuthUser>>();

            //builder.Register(c => new AuthUserStore()).AsImplementedInterfaces().InstancePerRequest();
            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication).As<IAuthenticationManager>();
            //builder.Register(c => new IdentityFactoryOptions<AuthUserManager>
            //{
            //    DataProtectionProvider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("Application​")
            //});
            builder.RegisterType<MachineKeyProtectionProvider>().As<IDataProtectionProvider>();

            // subscription 
            builder.RegisterType<ChargebeeSubscriptionRepository>().As<ISubscriptionRepository>();

            // Scene repository
            builder.RegisterType<ApplicationDbContext>().As<IApplicationDbContext>();
            builder.RegisterType<ApplicationDbContext>().As<IDeviceDbContext>();
            builder.RegisterType<ApplicationDbContext>().As<IKraniumDbContext>();
            builder.RegisterType<ApplicationDbContext>().As<ISceneDbContext>();
            builder.RegisterType<ApplicationDbContext>().As<IZoneDbContext>();
            builder.RegisterType<ApplicationDbContext>().As<IUserSettingsDbContext>();

            builder.RegisterType<EfDeviceRepository>().As<IDeviceRepository>().PropertiesAutowired();
            builder.RegisterType<EfKraniumRepository>().As<IKraniumRepository>().PropertiesAutowired();
            builder.RegisterType<EfSceneRepository>().As<ISceneRepository>().PropertiesAutowired();
            builder.RegisterType<EfUserSettingsRepository>().As<IUserSettingsRepository>().PropertiesAutowired();
            builder.RegisterType<EfZoneRepository>().As<IZoneRepository>().PropertiesAutowired();

            // Build the container and set the dependency resolver of the config.
            IContainer container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
