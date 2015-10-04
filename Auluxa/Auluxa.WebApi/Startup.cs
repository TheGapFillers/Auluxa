using System.Web.Http;
using Owin;
using Auluxa.Auth.Repositories;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using System;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.Google;

namespace Auluxa.WebApi
{
    /// <summary>
    /// Landing startup class. Start point of the service.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Method being called by the Owin layer to configure the application
        /// </summary>
        /// <param name="appBuilder">Injected appBuilder from the host.</param>
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            appBuilder.CreatePerOwinContext(AuthDbContext.Create);
            appBuilder.CreatePerOwinContext<AuthUserManager>(AuthUserManager.Create);
            appBuilder.CreatePerOwinContext<AuthSignInManager>(AuthSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            appBuilder.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<AuthUserManager, AuthUser>(
                        validateInterval: TimeSpan.FromMinutes(20),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            appBuilder.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            appBuilder.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            appBuilder.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Enable the application to use bearer tokens to authenticate users
            appBuilder.UseOAuthBearerTokens(new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                AuthorizeEndpointPath = new PathString("/Account/Authorize"),
                Provider = new OAuthProvider("web"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                AllowInsecureHttp = true
            });

            // Uncomment the following lines to enable logging in with third party login providers
            appBuilder.UseMicrosoftAccountAuthentication(
                clientId: "aaa",
                clientSecret: "aaa");

            appBuilder.UseTwitterAuthentication(
                consumerKey: "aaa",
                consumerSecret: "aaa");

            appBuilder.UseFacebookAuthentication(
                appId: "aaa",
                appSecret: "aaa");

            appBuilder.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "aaa",
                ClientSecret = "aaa"
            });

            // Web API configuration and services configuration
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            appBuilder.UseWebApi(config);
        }
    }
}
