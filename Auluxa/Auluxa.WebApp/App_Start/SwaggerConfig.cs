using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Web.Http;
using Auluxa.WebApp;
using WebActivatorEx;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]
namespace Auluxa.WebApp
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    // Manage title and versioning
                    c.SingleApiVersion("v1", ConfigurationManager.AppSettings["ServiceName"]);

                    // Automatically gets the generated XML doc of the controllers and inject them in swagger
                    string baseDirectory = AppDomain.CurrentDomain.RelativeSearchPath;

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
        }
    }
}
