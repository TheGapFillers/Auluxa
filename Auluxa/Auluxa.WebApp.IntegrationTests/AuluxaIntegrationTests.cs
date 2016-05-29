using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using Microsoft.Owin.Hosting;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Auluxa.WebApp.Auth;
using Auluxa.WebApp.IntegrationTests.Fakes;
using Xunit;

namespace Auluxa.WebApp.IntegrationTests
{
    public class TestServerFixture : IDisposable
    {
        private readonly IDisposable _testServer;

        public TestServerFixture()
        {
#if DEBUG
            _testServer = Microsoft.Owin.Hosting.WebApp.Start<Startup>(ConfigurationManager.AppSettings["auluxa-auth:Url"]);
#endif

            Trace.TraceInformation($"Starting integration tests on {ConfigurationManager.AppSettings["auluxa-auth:Url"]}");
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        }

        public void Dispose()
        {
            //delete the user if he wasn't deleted
            string email = "androiddev@8securities.com";
            string password = "Aszd1234";
            try
            {

            }
            catch
            {
                // ignored
            }

            // dispose the server
#if DEBUG
            _testServer.Dispose();
#endif
        }
    }

    [TestCaseOrderer("Auluxa.WebApp.IntegrationTests.Fakes.PriorityOrderer", "Auluxa.WebApp.IntegrationTests")]
    public class AuluxaIntegrationTests : TestServerFixture
    {
        [Fact, TestPriority(0)]
        public async Task test_get_token_valid_user()
        {
            // arrange
            string email = "ambroise.couissin@gmail.com";
            string password = "aaaa1111";

            // act
            OAuthToken token = await AuthProxy.LoginAsync(email, password);

            // assert
            Assert.NotNull(token.access_token);
        }

        /// <summary>
        /// GET /api/users/currentuser
        /// </summary>
        /// <returns></returns>
        [Fact, TestPriority(1)]
        public async Task test_get_current_user_is_valid()
        {
            // arrange
            HttpResponseMessage response;
            string email = "ambroise.couissin@gmail.com";
            string password = "aaaa1111";
            OAuthToken token = await AuthProxy.LoginAsync(email, password);

            // act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    token.token_type,
                    token.access_token);
                response = await httpClient.GetAsync(ConfigurationManager.AppSettings["auluxa-auth:Url"] + "api/users");
            }

            //assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            AuthUserViewModel user = await response.Content.ReadAsAsync<AuthUserViewModel>();
            Assert.Equal(user.Email, email);
        }

        /// <summary>
        /// GET /api/users/profiles
        /// </summary>
        /// <returns></returns>
        [Fact, TestPriority(2)]
        public async Task test_get_profiles_are_valid()
        {
            // arrange
            HttpResponseMessage response;
            string email = "ambroise.couissin@gmail.com";
            string password = "aaaa1111";
            OAuthToken token = await AuthProxy.LoginAsync(email, password);

            // act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    token.token_type,
                    token.access_token);
                response = await httpClient.GetAsync(ConfigurationManager.AppSettings["auluxa-auth:Url"] + "api/users/profiles");
            }

            //assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            IEnumerable<AuthUserViewModel> users = await response.Content.ReadAsAsync<IEnumerable<AuthUserViewModel>>();
            Assert.Equal(users.First().Email, email);
        }
    }
}
