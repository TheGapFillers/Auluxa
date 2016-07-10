using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Auluxa.WebApp.Auth;
using Auluxa.WebApp.Devices.Models;
using Auluxa.WebApp.IntegrationTests.Fakes;
using Microsoft.AspNet.Identity;
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
            string email = "ambroise.couissin@gmail.com";
            string password = "aaaa1111";
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
    public class AuluxaIntegrationTests : IClassFixture<TestServerFixture>
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
            UserViewModel user = await response.Content.ReadAsAsync<UserViewModel>();
            Assert.Equal(user.Email, email);
        }

        /// <summary>
        /// POST /api/users/profiles
        /// </summary>
        /// <returns></returns>
        [Fact, TestPriority(1)]
        public async Task test_create_profile_works()
        {
            // arrange
            HttpResponseMessage response;
            string email = "ambroise.couissin@gmail.com";
            string password = "aaaa1111";
            OAuthToken token = await AuthProxy.LoginAsync(email, password);
            var profile = new ProfileViewModel { Email = "profile1@gmail.com" };

            // act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    token.token_type,
                    token.access_token);
                response = await httpClient.PostAsJsonAsync(
                    ConfigurationManager.AppSettings["auluxa-auth:Url"] + "api/users/profiles",
                    profile);
            }

            //assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// GET /api/users/profiles
        /// </summary>
        /// <returns></returns>
        [Fact, TestPriority(3)]
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
                response =
                    await
                        httpClient.GetAsync(ConfigurationManager.AppSettings["auluxa-auth:Url"] + "api/users/profiles");
            }

            //assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            List<UserViewModel> users = (await response.Content.ReadAsAsync<IEnumerable<UserViewModel>>()).ToList();
            Assert.Equal(users.Count, 2);
            Assert.Equal(users[0].Email, email);
        }

        /// <summary>
        /// GET /api/devices
        /// </summary>
        /// <returns></returns>
        [Fact, TestPriority(4)]
        public async Task test_get_all_devices()
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
                response =
                    await httpClient.GetAsync(ConfigurationManager.AppSettings["auluxa-auth:Url"] + "api/devices");
            }

            //assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            IEnumerable<Device> devices = await response.Content.ReadAsAsync<IEnumerable<Device>>();
            Assert.Equal(2, devices.Count());
        }

        /// <summary>
        /// POST /api/devices
        /// </summary>
        /// <returns></returns>
        [Fact, TestPriority(5)]
        public async Task test_create_device()
        {
            // arrange
            HttpResponseMessage response;
            string email = "ambroise.couissin@gmail.com";
            string password = "aaaa1111";
            OAuthToken token = await AuthProxy.LoginAsync(email, password);
            var deviceToCreate = new CreateDeviceViewModel {DeviceModelId = 1};

            // act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    token.token_type,
                    token.access_token);
                response = await httpClient.PostAsJsonAsync(
                    ConfigurationManager.AppSettings["auluxa-auth:Url"] + "api/devices",
                    deviceToCreate);
            }

            //assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Device createdDevice = await response.Content.ReadAsAsync<Device>();
            Assert.NotNull(createdDevice);
        }

        /// <summary>
        /// PUT /api/devices
        /// </summary>
        /// <returns></returns>
        [Fact, TestPriority(6)]
        public async Task test_update_device_settings()
        {
            // arrange
            HttpResponseMessage response;
            string email = "ambroise.couissin@gmail.com";
            string password = "aaaa1111";
            OAuthToken token = await AuthProxy.LoginAsync(email, password);

            // get latest created device
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    token.token_type,
                    token.access_token);
                response =
                    await httpClient.GetAsync(ConfigurationManager.AppSettings["auluxa-auth:Url"] + "api/devices");
            }
            Device latestDevice = (await response.Content.ReadAsAsync<IEnumerable<Device>>()).Last();

            var deviceToUpdate = new UpdateDeviceSettingsViewModel
            {
                DeviceId = latestDevice.Id,
                Settings = new Dictionary<string, string>
                {
                    ["ACFunctionA"] = "FunctionAChoice2",
                    ["ACFunctionB"] = "FunctionBChoice3"
                }
            };

            // act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    token.token_type,
                    token.access_token);
                response = await httpClient.PutAsJsonAsync(
                    ConfigurationManager.AppSettings["auluxa-auth:Url"] + "api/devices/settings",
                    deviceToUpdate);
            }

            //assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Device createdDevice = await response.Content.ReadAsAsync<Device>();
            Assert.NotNull(createdDevice);
        }

        /// <summary>
        /// DELETE /api/devices
        /// </summary>
        /// <returns></returns>
        [Fact, TestPriority(7)]
        public async Task test_delete_device()
        {
            // arrange
            HttpResponseMessage response;
            string email = "ambroise.couissin@gmail.com";
            string password = "aaaa1111";
            OAuthToken token = await AuthProxy.LoginAsync(email, password);

            // get latest created device
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    token.token_type,
                    token.access_token);
                response =
                    await httpClient.GetAsync(ConfigurationManager.AppSettings["auluxa-auth:Url"] + "api/devices");
            }
            Device latestDevice = (await response.Content.ReadAsAsync<IEnumerable<Device>>()).Last();

            // act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    token.token_type,
                    token.access_token);
                response = await httpClient.DeleteAsync(
                    ConfigurationManager.AppSettings["auluxa-auth:Url"] + "api/devices/" + latestDevice.Id);
            }

            //assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Device createdDevice = await response.Content.ReadAsAsync<Device>();
            Assert.NotNull(createdDevice);
        }

        /// <summary>
        /// DELETE /api/users/profiles
        /// </summary>
        /// <returns></returns>
        [Fact, TestPriority(8)]
        public async Task test_delete_profile_works()
        {
            // arrange
            HttpResponseMessage response;
            string email = "ambroise.couissin@gmail.com";
            string password = "aaaa1111";
            OAuthToken token = await AuthProxy.LoginAsync(email, password);
            string emailToDelete = "profile1@gmail.com";

            // act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    token.token_type,
                    token.access_token);
                response = await httpClient.DeleteAsync(
                    $"{ConfigurationManager.AppSettings["auluxa-auth:Url"]}api/users/profiles?email={emailToDelete}");
            }

            // assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
