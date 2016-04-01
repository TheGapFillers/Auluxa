using System;
using System.Net.Http;
using Auluxa.WebApp.IntegrationTests.Hosts;
using NUnit.Framework;
using System.Net;
using Auluxa.WebApp.Zones.Models;
using Newtonsoft.Json;
using Auluxa.WebApp.IntegrationTests.Helpers;
using System.Net.Http.Formatting;

namespace Auluxa.WebApp.IntegrationTests
{
	public abstract class ZoneTestsImpl
	{ 
		private readonly IApiServer _server;

		private const string relativeUri = "api/zones";

		protected ZoneTestsImpl(IApiServer apiServer)
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
		public void Zones_GetAllZones()
		{
			Uri valuesUri = new Uri(_server.BaseAddress, relativeUri);
			using (HttpClient client = new HttpClient(_server.ServerHandler))
			using (HttpResponseMessage httpResponseMessage = client.GetAsync(valuesUri).Result)
			{
				Assert.IsTrue(httpResponseMessage.IsSuccessStatusCode);
				Assert.AreEqual(HttpStatusCode.OK, httpResponseMessage.StatusCode);

				Zone[] zones = JsonConvert.DeserializeObject<Zone[]>(httpResponseMessage.Content.ReadAsStringAsync().Result);
				Assert.AreEqual(2, zones.Length);
				Assert.AreEqual("Zone1", zones[0].Name);
				Assert.AreEqual("Bed Room", zones[1].Name);
			}
		}

		[TestCase("", 2)]
		[TestCase("0", 0)]
		[TestCase("1", 1)]
		[TestCase("1,2", 2)]
		[TestCase("2,3", 1)]
		public void Zones_GetZonesById(string ids, int expectedCount)
		{
			Uri valuesUri = new Uri(_server.BaseAddress, relativeUri + "?ids=" + ids);
			using (HttpClient client = new HttpClient(_server.ServerHandler))
			using (HttpResponseMessage httpResponseMessage = client.GetAsync(valuesUri).Result)
			{
				Assert.IsTrue(httpResponseMessage.IsSuccessStatusCode);
				Assert.AreEqual(HttpStatusCode.OK, httpResponseMessage.StatusCode);

				Zone[] zones = JsonConvert.DeserializeObject<Zone[]>(httpResponseMessage.Content.ReadAsStringAsync().Result);
				Assert.AreEqual(expectedCount, zones.Length);
			}
		}

		[Test]
		public void Zones_GetZonesById_InvalidFormatMustThrow()
		{
			Uri valuesUri = new Uri(_server.BaseAddress, relativeUri + "?ids=hahaha");
			using (HttpClient client = new HttpClient(_server.ServerHandler))
			using (HttpResponseMessage httpResponseMessage = client.GetAsync(valuesUri).Result)
			{
				Assert.IsFalse(httpResponseMessage.IsSuccessStatusCode);
				Assert.AreEqual(HttpStatusCode.BadRequest, httpResponseMessage.StatusCode);
				string content = httpResponseMessage.Content.ReadAsStringAsync().Result;
				Assert.AreEqual(String.Empty, content);
			}
		}

		[Test]
		public void Zones_PostZone()
		{
			Zone zoneToAdd = new Zone
			{
				Name = "New Zone Unlocked",
				UserName = "Alfred"
			};

			Uri valuesUri = new Uri(_server.BaseAddress, relativeUri);
			HttpRequestMessage request = HttpHelpers.CreateRequest<Zone>(valuesUri, "application/json", HttpMethod.Post, zoneToAdd, new JsonMediaTypeFormatter());
			using (HttpClient client = new HttpClient(_server.ServerHandler))
			using (HttpResponseMessage httpResponseMessage = client.SendAsync(request).Result)
			{
				Assert.IsTrue(httpResponseMessage.IsSuccessStatusCode);
				Assert.AreEqual(HttpStatusCode.Created, httpResponseMessage.StatusCode);

				Zone addedZone = JsonConvert.DeserializeObject<Zone>(httpResponseMessage.Content.ReadAsStringAsync().Result);
				Assert.AreEqual(3, addedZone.Id);
				Assert.AreEqual("New Zone Unlocked", addedZone.Name);
				Assert.AreEqual("Alfred", addedZone.UserName);
			}
		}

		[Test]
		public void Zones_PatchZone()
		{
			Zone zoneToPatch = new Zone
			{
				Id = 2,
				Name = "Renovated Bed Room",
			};

			Uri valuesUri = new Uri(_server.BaseAddress, relativeUri);

			// Patch and check returned result
			HttpRequestMessage request = HttpHelpers.CreateRequest<Zone>(valuesUri, "application/json", new HttpMethod("PATCH"), zoneToPatch, new JsonMediaTypeFormatter());
			using (HttpClient client = new HttpClient(_server.ServerHandler))
			using (HttpResponseMessage httpResponseMessage = client.SendAsync(request).Result)
			{
				Assert.IsTrue(httpResponseMessage.IsSuccessStatusCode);
				Assert.AreEqual(HttpStatusCode.Created, httpResponseMessage.StatusCode);

				Zone modifiedZone = JsonConvert.DeserializeObject<Zone>(httpResponseMessage.Content.ReadAsStringAsync().Result);
				Assert.AreEqual(2, modifiedZone.Id);
				Assert.AreEqual("Renovated Bed Room", modifiedZone.Name);
				Assert.Null(modifiedZone.UserName);
			}

			// Make sure patch has been applied
			valuesUri = new Uri(_server.BaseAddress, relativeUri + "?ids=2");
			request = HttpHelpers.CreateRequest(valuesUri, "application/json", HttpMethod.Get);
			using (HttpClient client = new HttpClient(_server.ServerHandler))
			using (HttpResponseMessage httpResponseMessage = client.SendAsync(request).Result)
			{
				Assert.IsTrue(httpResponseMessage.IsSuccessStatusCode);
				Assert.AreEqual(HttpStatusCode.OK, httpResponseMessage.StatusCode);

				Zone[] zones = JsonConvert.DeserializeObject<Zone[]>(httpResponseMessage.Content.ReadAsStringAsync().Result);
				Assert.AreEqual(1, zones.Length);
				Assert.AreEqual(2, zones[0].Id);
				Assert.AreEqual("Renovated Bed Room", zones[0].Name);
				Assert.Null(zones[0].UserName);
			}
		}

		[Test]
		public void Zones_DeleteZone()
		{
			int idOfZoneToDelete = 1;
			Uri valuesUri = new Uri(_server.BaseAddress, relativeUri + "/" + idOfZoneToDelete);

			// Make sure the zone exists initially
			HttpRequestMessage request = HttpHelpers.CreateRequest(valuesUri, "application/json", HttpMethod.Get);
			using (HttpClient client = new HttpClient(_server.ServerHandler))
			using (HttpResponseMessage httpResponseMessage = client.SendAsync(request).Result)
			{
				Assert.IsTrue(httpResponseMessage.IsSuccessStatusCode);
				Assert.AreEqual(HttpStatusCode.OK, httpResponseMessage.StatusCode);

				Zone[] zones = JsonConvert.DeserializeObject<Zone[]>(httpResponseMessage.Content.ReadAsStringAsync().Result);
				Assert.AreEqual(1, zones.Length);
			}

			// Delete it
			request = HttpHelpers.CreateRequest(valuesUri, "application/json", HttpMethod.Delete);
			using (HttpClient client = new HttpClient(_server.ServerHandler))
			using (HttpResponseMessage httpResponseMessage = client.SendAsync(request).Result)
			{
				Assert.IsTrue(httpResponseMessage.IsSuccessStatusCode);
				Assert.AreEqual(HttpStatusCode.OK, httpResponseMessage.StatusCode);

				Zone deletedZone = JsonConvert.DeserializeObject<Zone>(httpResponseMessage.Content.ReadAsStringAsync().Result);
				Assert.AreEqual(idOfZoneToDelete, deletedZone.Id);
				Assert.AreEqual("Zone1", deletedZone.Name);
				Assert.IsNull(deletedZone.UserName);
			}

			// Make sure the zone can't be retrieved again
			using (HttpClient client = new HttpClient(_server.ServerHandler))
			using (HttpResponseMessage httpResponseMessage = client.GetAsync(valuesUri).Result)
			{
				Assert.IsTrue(httpResponseMessage.IsSuccessStatusCode);
				Assert.AreEqual(HttpStatusCode.OK, httpResponseMessage.StatusCode);

				Zone[] zones = JsonConvert.DeserializeObject<Zone[]>(httpResponseMessage.Content.ReadAsStringAsync().Result);
				Assert.AreEqual(0, zones.Length);
			}
		}
	}
}
