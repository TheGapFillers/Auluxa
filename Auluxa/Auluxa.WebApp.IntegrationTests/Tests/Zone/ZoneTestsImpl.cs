using System;
using System.Net.Http;
using Auluxa.WebApp.IntegrationTests.Hosts;
using NUnit.Framework;
using System.Net;
using Auluxa.WebApp.Zones.Models;
using Newtonsoft.Json;
using Auluxa.WebApp.IntegrationTests.Helpers;
using System.Net.Http.Formatting;
using System.Linq;

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
		public void Zone_1_GetAllZones()
		{
			Uri valuesUri = new Uri(_server.BaseAddress, relativeUri);
			Zone[] zones = HttpHelpers.GetEntities<Zone>(valuesUri, _server.ServerHandler);

			Assert.AreEqual(2, zones.Length);

			Zone retrievedZone = zones.Where(z => z.Id == 1).SingleOrDefault();
			Assert.AreEqual("Zone1", retrievedZone.Name);
			retrievedZone = zones.Where(z => z.Id == 2).SingleOrDefault();
			Assert.AreEqual("Bed Room", retrievedZone.Name);
		}

		[TestCase("", 2)]
		[TestCase("0", 0)]
		[TestCase("1", 1)]
		[TestCase("1,2", 2)]
		[TestCase("2,3", 1)]
		public void Zone_1_GetZonesById(string ids, int expectedCount)
		{
			Uri valuesUri = new Uri(_server.BaseAddress, relativeUri + "?ids=" + ids);
			Zone[] zones = HttpHelpers.GetEntities<Zone>(valuesUri, _server.ServerHandler);

			Assert.AreEqual(expectedCount, zones.Length);
		}

		[Test]
		public void Zones_1_GetZonesById_InvalidFormat_MustReturnBadRequest()
		{
			Uri valuesUri = new Uri(_server.BaseAddress, relativeUri + "?ids=hahaha");
			Zone[] zones = HttpHelpers.GetEntities<Zone>(valuesUri, _server.ServerHandler, false, HttpStatusCode.BadRequest);
			Assert.IsNull(zones);
		}

		[Test]
		public void Zone_2_PostZone()
		{
			Zone zoneToAdd = new Zone
			{
				Name = "New Zone Unlocked",
				UserName = "Alfred"
			};
			int createdZoneId;

			// Post and check it returns what we sent
			Uri valuesUri = new Uri(_server.BaseAddress, relativeUri);
			Zone createdZone = HttpHelpers.PostEntities<Zone>(valuesUri, _server.ServerHandler, zoneToAdd, true, HttpStatusCode.Created);
			Assert.AreEqual("New Zone Unlocked", createdZone.Name);
			Assert.AreEqual("Alfred", createdZone.UserName);

			createdZoneId = createdZone.Id;

			// Retrieve manually and make sure it has been saved
			valuesUri = new Uri(_server.BaseAddress, relativeUri + "?ids=" + createdZoneId);
			Zone retrievedZone = HttpHelpers.GetEntities<Zone>(valuesUri, _server.ServerHandler).SingleOrDefault();
			Assert.IsNotNull(retrievedZone);
			Assert.AreEqual("New Zone Unlocked", retrievedZone.Name);
			Assert.AreEqual("Alfred", retrievedZone.UserName);
		}

		[Test]
		public void Zone_2_PatchZone()
		{
			Zone zoneToPatch = new Zone
			{
				Id = 2,
				Name = "Renovated Bed Room",
				UserName = "Batman",
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
				Assert.AreEqual("Batman", modifiedZone.UserName);
			}

			// Get modified Zone and make sure patch has been applied
			valuesUri = new Uri(_server.BaseAddress, relativeUri + "?ids=2");
			Zone retrievedZone = HttpHelpers.GetEntities<Zone>(valuesUri, _server.ServerHandler).SingleOrDefault();
			
			Assert.AreEqual(2, retrievedZone.Id);
			Assert.AreEqual("Renovated Bed Room", retrievedZone.Name);
			Assert.AreEqual("Batman", retrievedZone.UserName);
		}

		[Test]
		public void Zone_3_DeleteZone()
		{
			int idOfZoneToDelete = 1;

			// Make sure the zone exists initially
			Uri valuesUri = new Uri(_server.BaseAddress, relativeUri + "?ids=" + idOfZoneToDelete);
			Zone existingZone = HttpHelpers.GetEntities<Zone>(valuesUri, _server.ServerHandler).SingleOrDefault();
			Assert.IsNotNull(existingZone);
			
			// Delete it
			valuesUri = new Uri(_server.BaseAddress, relativeUri + "/" + idOfZoneToDelete);
			HttpRequestMessage request = HttpHelpers.CreateRequest(valuesUri, "application/json", HttpMethod.Delete);
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
			valuesUri = new Uri(_server.BaseAddress, relativeUri + "?ids=" + idOfZoneToDelete);
			Zone alreadyDeletedZone = HttpHelpers.GetEntities<Zone>(valuesUri, _server.ServerHandler).SingleOrDefault();
			Assert.IsNull(alreadyDeletedZone);
		}
	}
}
