using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Auluxa.WebApp.Auth
{
    public static class AuthProxy
    {
        public static async Task<OAuthToken> LoginAsync(string username, string password)
        {
            using (var httpClient = new HttpClient())
            {
                var httpContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("client_id", "web"),
                    new KeyValuePair<string, string>("client_secret", "nosecret"),
                    new KeyValuePair<string, string>("username", username),
                    new KeyValuePair<string, string>("password", password),
                });

                HttpResponseMessage response = httpClient.PostAsync(
                    ConfigurationManager.AppSettings["auluxa-auth:Url"] + "token",
                    httpContent).Result;

                response.EnsureSuccessStatusCode();

                OAuthToken token = await response.Content.ReadAsAsync<OAuthToken>();
                return token;
            }
        }
    }
}