using System;
using System.Net.Http;
using Auluxa.WebApp.IntegrationTests.Hosts;
using NUnit.Framework;
using System.Net;
using Newtonsoft.Json;
using Auluxa.WebApp.IntegrationTests.Helpers;
using System.Net.Http.Formatting;
using System.Linq;
using Auluxa.WebApp.Appliances.Models;
using System.Collections.Generic;

namespace Auluxa.WebApp.IntegrationTests
{
	public abstract class ApplianceTestsImpl
	{ 
		private readonly IApiServer _server;

		private const string relativeUri = "api/appliances";

		protected ApplianceTestsImpl(IApiServer apiServer)
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
		public void Appliance_GetAllAppliances()
		{
			Uri valuesUri = new Uri(_server.BaseAddress, relativeUri);
			Appliance[] appliances = HttpHelpers.GetEntities<Appliance>(valuesUri, _server.ServerHandler);

			Assert.AreEqual(2, appliances.Length);

			Appliance retrievedAppliance = appliances.Where(z => z.Id == 1).SingleOrDefault();
			Assert.AreEqual("Appliance1", retrievedAppliance.Name);
			retrievedAppliance = appliances.Where(z => z.Id == 2).SingleOrDefault();
			Assert.AreEqual("Appliance2", retrievedAppliance.Name);
		}

		[TestCase("", 2)]
		[TestCase("0", 0)]
		[TestCase("1", 1)]
		[TestCase("1,2", 2)]
		[TestCase("2,3", 1)]
		public void Appliance_GetApplianceById(string ids, int expectedCount)
		{
			Uri valuesUri = new Uri(_server.BaseAddress, relativeUri + "?ids=" + ids);
			Appliance[] appliances = HttpHelpers.GetEntities<Appliance>(valuesUri, _server.ServerHandler);

			Assert.AreEqual(expectedCount, appliances.Length);
		}

		[Test]
		public void Appliance_GetApplianceById_InvalidFormat_MustReturnBadRequest()
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
		public void Appliance_PostAppliance_UseDefaultSettings()
		{
			Appliance applianceToAdd = new Appliance
			{
				Name = "New AC",
				UserName = "Alfred",
				Model = new ApplianceModel { Id = 1 }
			};
			int createdApplianceId;

			// Post and check it returns what we sent
			Uri valuesUri = new Uri(_server.BaseAddress, relativeUri);
			HttpRequestMessage request = HttpHelpers.CreateRequest<Appliance>(valuesUri, "application/json", HttpMethod.Post, applianceToAdd, new JsonMediaTypeFormatter());
			using (HttpClient client = new HttpClient(_server.ServerHandler))
			using (HttpResponseMessage httpResponseMessage = client.SendAsync(request).Result)
			{
				Assert.IsTrue(httpResponseMessage.IsSuccessStatusCode);
				Assert.AreEqual(HttpStatusCode.Created, httpResponseMessage.StatusCode);

				Appliance createdAppliance = JsonConvert.DeserializeObject<Appliance>(httpResponseMessage.Content.ReadAsStringAsync().Result);
				Assert.AreEqual("New AC", createdAppliance.Name);
				Assert.AreEqual("Alfred", createdAppliance.UserName);
				Assert.AreEqual(1, createdAppliance.Model.Id);
				Assert.AreEqual("FunctionADefaultChoice", createdAppliance.CurrentSetting["functionA"]);	//todo for some reason first letter gets lowercased...
				Assert.AreEqual("FunctionBDefaultChoice", createdAppliance.CurrentSetting["functionB"]);

				createdApplianceId = createdAppliance.Id;
			}

			// Retrieve manually and make sure it has been saved
			valuesUri = new Uri(_server.BaseAddress, relativeUri + "?ids=" + createdApplianceId);
			Appliance retrievedAppliancee = HttpHelpers.GetEntities<Appliance>(valuesUri, _server.ServerHandler).SingleOrDefault();
			Assert.IsNotNull(retrievedAppliancee);
			Assert.AreEqual("New AC", retrievedAppliancee.Name);
			Assert.AreEqual("Alfred", retrievedAppliancee.UserName);
			Assert.AreEqual(1, retrievedAppliancee.Model.Id);
			Assert.AreEqual("FunctionADefaultChoice", retrievedAppliancee.CurrentSetting["functionA"]);    //todo for some reason first letter gets lowercased...
			Assert.AreEqual("FunctionBDefaultChoice", retrievedAppliancee.CurrentSetting["functionB"]);
		}

		[Test]
		public void Appliance_PostAppliance_SpecifySettingsManually()
		{
			Appliance applianceToAdd = new Appliance
			{
				Name = "New AC",
				UserName = "Alfred",
				Model = new ApplianceModel { Id = 1 },
				CurrentSetting = new Dictionary<string, string>
				{
					["FunctionA"] = "FunctionAChoice2",
					["FunctionB"] = "FunctionBChoice3"
				}
			};
			int createdApplianceId;

			Uri valuesUri = new Uri(_server.BaseAddress, relativeUri);
			HttpRequestMessage request = HttpHelpers.CreateRequest<Appliance>(valuesUri, "application/json", HttpMethod.Post, applianceToAdd, new JsonMediaTypeFormatter());
			using (HttpClient client = new HttpClient(_server.ServerHandler))
			using (HttpResponseMessage httpResponseMessage = client.SendAsync(request).Result)
			{
				Assert.IsTrue(httpResponseMessage.IsSuccessStatusCode);
				Assert.AreEqual(HttpStatusCode.Created, httpResponseMessage.StatusCode);

				Appliance createdAppliance = JsonConvert.DeserializeObject<Appliance>(httpResponseMessage.Content.ReadAsStringAsync().Result);
				Assert.AreEqual("New AC", createdAppliance.Name);
				Assert.AreEqual("Alfred", createdAppliance.UserName);
				Assert.AreEqual(1, createdAppliance.Model.Id);
				Assert.AreEqual("FunctionAChoice2", createdAppliance.CurrentSetting["functionA"]);  //todo for some reason first letter gets lowercased...
				Assert.AreEqual("FunctionBChoice3", createdAppliance.CurrentSetting["functionB"]);

				createdApplianceId = createdAppliance.Id;
			}

			// Retrieve manually and make sure it has been saved
			valuesUri = new Uri(_server.BaseAddress, relativeUri + "?ids=" + createdApplianceId);
			Appliance retrievedAppliancee = HttpHelpers.GetEntities<Appliance>(valuesUri, _server.ServerHandler).SingleOrDefault();
			Assert.IsNotNull(retrievedAppliancee);
			Assert.AreEqual("New AC", retrievedAppliancee.Name);
			Assert.AreEqual("Alfred", retrievedAppliancee.UserName);
			Assert.AreEqual(1, retrievedAppliancee.Model.Id);
			Assert.AreEqual("FunctionAChoice2", retrievedAppliancee.CurrentSetting["functionA"]);    //todo for some reason first letter gets lowercased...
			Assert.AreEqual("FunctionBChoice3", retrievedAppliancee.CurrentSetting["functionB"]);
		}

		[Test]
		public void Appliance_PostAppliance_SpecifyIncompleteSettingsManually_MustReturnBadRequest()
		{
			Appliance applianceToAdd = new Appliance
			{
				Name = "New AC",
				UserName = "Alfred",
				Model = new ApplianceModel { Id = 1 },
				CurrentSetting = new Dictionary<string, string>
				{
					["FunctionA"] = "FunctionAChoice2",
					//["FunctionB"] = "FunctionBChoice3" // We skip one setting
				}
			};

			Uri valuesUri = new Uri(_server.BaseAddress, relativeUri);
			HttpRequestMessage request = HttpHelpers.CreateRequest<Appliance>(valuesUri, "application/json", HttpMethod.Post, applianceToAdd, new JsonMediaTypeFormatter());
			using (HttpClient client = new HttpClient(_server.ServerHandler))
			using (HttpResponseMessage httpResponseMessage = client.SendAsync(request).Result)
			{
				Assert.IsFalse(httpResponseMessage.IsSuccessStatusCode);
				Assert.AreEqual(HttpStatusCode.InternalServerError, httpResponseMessage.StatusCode); 
				//todo should not be Error 500. Change controller/repo to follow best practices
			}
		}

		[Test]
		public void Appliance_PostAppliance_SpecifyWrongSettingsManually_MustReturnBadRequest()
		{
			Appliance applianceToAdd = new Appliance
			{
				Name = "New AC",
				UserName = "Alfred",
				Model = new ApplianceModel { Id = 1 },
				CurrentSetting = new Dictionary<string, string>
				{
					["FunctionA"] = "FunctionAChoice2",
					["FunctionB"] = "AWrongSetting"
				}
			};

			Uri valuesUri = new Uri(_server.BaseAddress, relativeUri);
			HttpRequestMessage request = HttpHelpers.CreateRequest<Appliance>(valuesUri, "application/json", HttpMethod.Post, applianceToAdd, new JsonMediaTypeFormatter());
			using (HttpClient client = new HttpClient(_server.ServerHandler))
			using (HttpResponseMessage httpResponseMessage = client.SendAsync(request).Result)
			{
				Assert.IsFalse(httpResponseMessage.IsSuccessStatusCode);
				Assert.AreEqual(HttpStatusCode.InternalServerError, httpResponseMessage.StatusCode);
				//todo should not be Error 500. Change controller/repo to follow best practices
			}
		}

		[Test]
		public void Appliance_PostAppliance_NoModelSpecified_MustReturnBadRequest()
		{
			Appliance applianceToAdd = new Appliance
			{
				Name = "New AC",
				UserName = "Alfred",
				CurrentSetting = new Dictionary<string, string>
				{
					["FunctionA"] = "FunctionAChoice2",
					["FunctionB"] = "AWrongSetting"
				}
			};

			Uri valuesUri = new Uri(_server.BaseAddress, relativeUri);
			HttpRequestMessage request = HttpHelpers.CreateRequest<Appliance>(valuesUri, "application/json", HttpMethod.Post, applianceToAdd, new JsonMediaTypeFormatter());
			using (HttpClient client = new HttpClient(_server.ServerHandler))
			using (HttpResponseMessage httpResponseMessage = client.SendAsync(request).Result)
			{
				Assert.IsFalse(httpResponseMessage.IsSuccessStatusCode);
				Assert.AreEqual(HttpStatusCode.InternalServerError, httpResponseMessage.StatusCode);
				//todo should not be Error 500. Change controller/repo to follow best practices
			}
		}

		[Test]
		public void Appliance_PatchAppliance()
		{
			Appliance applianceToPatch = new Appliance
			{
				Id = 2,
				Name = "Patched AC",
				UserName = "Batman",
			};

			Uri valuesUri = new Uri(_server.BaseAddress, relativeUri);

			// Patch and check returned result
			HttpRequestMessage request = HttpHelpers.CreateRequest<Appliance>(valuesUri, "application/json", new HttpMethod("PATCH"), applianceToPatch, new JsonMediaTypeFormatter());
			using (HttpClient client = new HttpClient(_server.ServerHandler))
			using (HttpResponseMessage httpResponseMessage = client.SendAsync(request).Result)
			{
				Assert.IsTrue(httpResponseMessage.IsSuccessStatusCode);
				Assert.AreEqual(HttpStatusCode.Created, httpResponseMessage.StatusCode);

				Appliance modifiedAppliance = JsonConvert.DeserializeObject<Appliance>(httpResponseMessage.Content.ReadAsStringAsync().Result);
				Assert.AreEqual(2, modifiedAppliance.Id);
				Assert.AreEqual("Patched AC", modifiedAppliance.Name);
				Assert.AreEqual("Batman", modifiedAppliance.UserName);
			}

			// Get modified Appliance and make sure patch has been applied
			valuesUri = new Uri(_server.BaseAddress, relativeUri + "?ids=2");
			Appliance retrievedAppliancee = HttpHelpers.GetEntities<Appliance>(valuesUri, _server.ServerHandler).SingleOrDefault();

			Assert.IsNotNull(retrievedAppliancee);
			Assert.AreEqual(2, retrievedAppliancee.Id);
			Assert.AreEqual("Patched AC", retrievedAppliancee.Name);
			Assert.AreEqual("Batman", retrievedAppliancee.UserName);
		}

		//todo setting and model patch

		//todo zone patch /!\ HAVE ISSUES HERE

		[Test]
		public void Appliance_DeleteAppliance()
		{
			int idOfApplianceToDelete = 1;

			// Make sure the appliance exists initially
			Uri valuesUri = new Uri(_server.BaseAddress, relativeUri + "?ids=" + idOfApplianceToDelete);
			Appliance existingAppliancee = HttpHelpers.GetEntities<Appliance>(valuesUri, _server.ServerHandler).SingleOrDefault();
			Assert.IsNotNull(existingAppliancee);

			// Delete it
			valuesUri = new Uri(_server.BaseAddress, relativeUri + "/" + idOfApplianceToDelete);
			HttpRequestMessage request = HttpHelpers.CreateRequest(valuesUri, "application/json", HttpMethod.Delete);
			using (HttpClient client = new HttpClient(_server.ServerHandler))
			using (HttpResponseMessage httpResponseMessage = client.SendAsync(request).Result)
			{
				Assert.IsTrue(httpResponseMessage.IsSuccessStatusCode);
				Assert.AreEqual(HttpStatusCode.OK, httpResponseMessage.StatusCode);

				Appliance deletedAppliance = JsonConvert.DeserializeObject<Appliance>(httpResponseMessage.Content.ReadAsStringAsync().Result);
				Assert.AreEqual(idOfApplianceToDelete, deletedAppliance.Id);
				Assert.AreEqual("Appliance1", deletedAppliance.Name);
				Assert.AreEqual("Serge", deletedAppliance.UserName);
			}

			// Make sure the appliance can't be retrieved again
			valuesUri = new Uri(_server.BaseAddress, relativeUri + "?ids=" + idOfApplianceToDelete);
			Appliance alreadyDeletedgAppliancee = HttpHelpers.GetEntities<Appliance>(valuesUri, _server.ServerHandler).SingleOrDefault();
			Assert.IsNull(alreadyDeletedgAppliancee);
		}
	}
}
