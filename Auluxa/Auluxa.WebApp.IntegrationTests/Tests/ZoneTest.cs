using System;
using System.Net;
using System.Net.Http;
using Auluxa.WebApp.IntegrationTests.Server;
using NUnit.Framework;

namespace Auluxa.WebApp.IntegrationTests.Tests
{
    public abstract class ZoneTest
    { 
        private readonly IApiServer _server;

        private const string relativeUri = "api/zones";

        protected ZoneTest(IApiServer apiServer)
        {
            _server = apiServer;
        }

        [SetUp]
        public void Setup()
        {
            _server.Start();
        }

        [TearDown]
        public void TearDown()
        {
            _server.Stop();
        }

        [Test]
        public void GetAllZones()
        {
            var valuesUri = new Uri(_server.BaseAddress, relativeUri);
            using (var client = new HttpClient(_server.ServerHandler))
            {
                HttpResponseMessage httpResponseMessage = client.GetAsync(valuesUri).Result;
                Assert.That(httpResponseMessage.IsSuccessStatusCode);
                Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
				string actual = httpResponseMessage.Content.ReadAsStringAsync().Result;
				Assert.AreEqual("", actual);
			}
        }

        //[Test]
        //public void CanSuccessfullyGetAllBooks()
        //{
        //    var valuesUri = new Uri(_server.BaseAddress, relativeUri);
        //    using (var client = new HttpClient(_server.ServerHandler))
        //    {
        //        HttpResponseMessage httpResponseMessage = client.GetAsync(valuesUri).Result;
        //        dynamic result = httpResponseMessage.Content.ReadAsAsync<ExpandoObject>().Result;
        //        Assert.That(result, Is.Not.Null);
        //        Assert.That(Enumerable.Any(result.Catalog));
        //    }
        //}
    }
}
