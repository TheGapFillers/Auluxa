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
using Auluxa.WebApp.Zones.Models;

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
		public void Appliance_1_GetAllAppliances()
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
		public void Appliance_1_GetApplianceById(string ids, int expectedCount)
		{
			Uri valuesUri = new Uri(_server.BaseAddress, relativeUri + "?ids=" + ids);
			Appliance[] appliances = HttpHelpers.GetEntities<Appliance>(valuesUri, _server.ServerHandler);

			Assert.AreEqual(expectedCount, appliances.Length);
		}

		[Test]
		public void Appliance_1_GetApplianceById_InvalidFormat_MustReturnBadRequest()
		{
			Uri valuesUri = new Uri(_server.BaseAddress, relativeUri + "?ids=hahaha");
			Appliance[] appliances = HttpHelpers.GetEntities<Appliance>(valuesUri, _server.ServerHandler, false, HttpStatusCode.BadRequest);
			Assert.IsNull(appliances);
		}

		[Test]
		public void Appliance_2_PostAppliance_UseDefaultSettings()
		{
			Appliance applianceToAdd = new Appliance
			{
				Name = "New AC",
				UserName = "Alfred",
				Model = new ApplianceModel { Id = 1 }
			};

			// Post and check it returns what we sent
			Uri valuesUri = new Uri(_server.BaseAddress, relativeUri);
			Appliance createdAppliance = HttpHelpers.PostEntities<Appliance>(valuesUri, _server.ServerHandler, applianceToAdd, true, HttpStatusCode.Created);
			Assert.AreEqual("New AC", createdAppliance.Name);
			Assert.AreEqual("Alfred", createdAppliance.UserName);
			Assert.AreEqual(1, createdAppliance.Model.Id);
			Assert.AreEqual("FunctionADefaultChoice", createdAppliance.CurrentSetting["acFunctionA"]);  //todo for some reason first letter gets lowercased...
			Assert.AreEqual("FunctionBDefaultChoice", createdAppliance.CurrentSetting["acFunctionB"]);

			int createdApplianceId = createdAppliance.Id;
			
			// Retrieve manually and make sure it has been saved
			valuesUri = new Uri(_server.BaseAddress, relativeUri + "?ids=" + createdApplianceId);
			Appliance retrievedAppliancee = HttpHelpers.GetEntities<Appliance>(valuesUri, _server.ServerHandler).SingleOrDefault();
			Assert.IsNotNull(retrievedAppliancee);
			Assert.AreEqual("New AC", retrievedAppliancee.Name);
			Assert.AreEqual("Alfred", retrievedAppliancee.UserName);
			Assert.AreEqual(1, retrievedAppliancee.Model.Id);
			Assert.AreEqual("FunctionADefaultChoice", retrievedAppliancee.CurrentSetting["acFunctionA"]);    //todo for some reason first letter gets lowercased...
			Assert.AreEqual("FunctionBDefaultChoice", retrievedAppliancee.CurrentSetting["acFunctionB"]);
		}

		[Test]
		public void Appliance_2_PostAppliance_SpecifySettingsManually()
		{
			Appliance applianceToAdd = new Appliance
			{
				Name = "New AC",
				UserName = "Alfred",
				Model = new ApplianceModel { Id = 1 },
				CurrentSetting = new Dictionary<string, string>
				{
					["ACFunctionA"] = "FunctionAChoice2",
					["ACFunctionB"] = "FunctionBChoice2"
				}
			};

			Uri valuesUri = new Uri(_server.BaseAddress, relativeUri);
			Appliance createdAppliance = HttpHelpers.PostEntities<Appliance>(valuesUri, _server.ServerHandler, applianceToAdd, true, HttpStatusCode.Created);
			Assert.AreEqual("New AC", createdAppliance.Name);
			Assert.AreEqual("Alfred", createdAppliance.UserName);
			Assert.AreEqual(1, createdAppliance.Model.Id);
			Assert.AreEqual("FunctionAChoice2", createdAppliance.CurrentSetting["acFunctionA"]);  //todo for some reason first letter gets lowercased...
			Assert.AreEqual("FunctionBChoice2", createdAppliance.CurrentSetting["acFunctionB"]);

			int createdApplianceId = createdAppliance.Id;
		
			// Retrieve manually and make sure it has been saved
			valuesUri = new Uri(_server.BaseAddress, relativeUri + "?ids=" + createdApplianceId);
			Appliance retrievedAppliancee = HttpHelpers.GetEntities<Appliance>(valuesUri, _server.ServerHandler).SingleOrDefault();
			Assert.IsNotNull(retrievedAppliancee);
			Assert.AreEqual("New AC", retrievedAppliancee.Name);
			Assert.AreEqual("Alfred", retrievedAppliancee.UserName);
			Assert.AreEqual(1, retrievedAppliancee.Model.Id);
			Assert.AreEqual("FunctionAChoice2", retrievedAppliancee.CurrentSetting["acFunctionA"]);    //todo for some reason first letter gets lowercased...
			Assert.AreEqual("FunctionBChoice2", retrievedAppliancee.CurrentSetting["acFunctionB"]);
		}

		[Test]
		public void Appliance_2_PostAppliance_SpecifyIncompleteSettingsManually_MustReturnBadRequest()
		{
			Appliance applianceToAdd = new Appliance
			{
				Name = "New AC",
				UserName = "Alfred",
				Model = new ApplianceModel { Id = 1 },
				CurrentSetting = new Dictionary<string, string>
				{
					["ACFunctionA"] = "FunctionAChoice2",
					//["acFunctionB"] = "FunctionBChoice2" // We skip one setting
				}
			};

			Uri valuesUri = new Uri(_server.BaseAddress, relativeUri);
			Appliance createdAppliance = HttpHelpers.PostEntities<Appliance>(valuesUri, _server.ServerHandler, applianceToAdd, false, HttpStatusCode.InternalServerError);
			//todo should not be Error 500. Change controller/repo to follow best practices
			Assert.IsNull(createdAppliance);
		}

		[Test]
		public void Appliance_2_PostAppliance_SpecifyWrongSettingsManually_MustReturnBadRequest()
		{
			Appliance applianceToAdd = new Appliance
			{
				Name = "New AC",
				UserName = "Alfred",
				Model = new ApplianceModel { Id = 1 },
				CurrentSetting = new Dictionary<string, string>
				{
					["ACFunctionA"] = "FunctionAChoice2",
					["ACFunctionB"] = "AWrongSetting"
				}
			};

			Uri valuesUri = new Uri(_server.BaseAddress, relativeUri);
			Appliance createdAppliance = HttpHelpers.PostEntities<Appliance>(valuesUri, _server.ServerHandler, applianceToAdd, false, HttpStatusCode.InternalServerError);
			//todo should not be Error 500. Change controller/repo to follow best practices
			Assert.IsNull(createdAppliance);
		}

		[Test]
		public void Appliance_2_PostAppliance_NoModelSpecified_MustReturnBadRequest()
		{
			Appliance applianceToAdd = new Appliance
			{
				Name = "New AC",
				UserName = "Alfred",
				CurrentSetting = new Dictionary<string, string>
				{
					["ACFunctionA"] = "FunctionAChoice2",
					["ACFunctionB"] = "FunctionBChoice2"
				}
			};

			Uri valuesUri = new Uri(_server.BaseAddress, relativeUri);
			Appliance createdAppliance = HttpHelpers.PostEntities<Appliance>(valuesUri, _server.ServerHandler, applianceToAdd, false, HttpStatusCode.InternalServerError);
			//todo should not be Error 500. Change controller/repo to follow best practices
			Assert.IsNull(createdAppliance);
		}

		[Test]
		public void Appliance_3_PatchAppliance_CompleteAndUseDefaultSettings()
		{
			Appliance applianceToPatch = new Appliance
			{
				Id = 2,
				Name = "Patched AC",
				UserName = "Batman",
				Zone = new Zone { Id = 1 },
				Model = new ApplianceModel { Id = 2 },
			};

			Uri valuesUri = new Uri(_server.BaseAddress, relativeUri);

			// Patch and check returned result
			HttpRequestMessage request = HttpHelpers.CreateRequest<Appliance>(valuesUri, "application/json", new HttpMethod("PATCH"), applianceToPatch, new JsonMediaTypeFormatter());
			Appliance modifiedAppliance = HttpHelpers.PatchEntities<Appliance>(valuesUri, _server.ServerHandler, applianceToPatch, true, HttpStatusCode.Created);

			Assert.AreEqual(2, modifiedAppliance.Id, "Invalid Id");
			Assert.AreEqual("Patched AC", modifiedAppliance.Name, "Invalid Name");
			Assert.AreEqual("Batman", modifiedAppliance.UserName, "Invalid UserName");
			Assert.AreEqual(2, modifiedAppliance.Model.Id, "Invalid Model Id, test might have been ran in wrong order");
			Assert.AreEqual("FunctionADefaultChoice", modifiedAppliance.CurrentSetting["lightFunctionA"]);    //todo for some reason first letter gets lowercased...
			Assert.AreEqual("FunctionBDefaultChoice", modifiedAppliance.CurrentSetting["lightFunctionB"]);
			Assert.AreEqual(1, modifiedAppliance.Zone.Id, "Invalid Zone Id");

			// Get modified Appliance and make sure patch has been applied
			valuesUri = new Uri(_server.BaseAddress, relativeUri + "?ids=2");
			Appliance retrievedAppliancee = HttpHelpers.GetEntities<Appliance>(valuesUri, _server.ServerHandler).SingleOrDefault();

			Assert.IsNotNull(retrievedAppliancee);
			Assert.AreEqual(2, retrievedAppliancee.Id, "Invalid Id");
			Assert.AreEqual("Patched AC", retrievedAppliancee.Name, "Invalid Name");
			Assert.AreEqual("Batman", retrievedAppliancee.UserName, "Invalid UserName");
			Assert.AreEqual(2, retrievedAppliancee.Model.Id, "Invalid Model Id, test might have been ran in wrong order");
			Assert.AreEqual("FunctionADefaultChoice", retrievedAppliancee.CurrentSetting["lightFunctionA"]);    //todo for some reason first letter gets lowercased...
			Assert.AreEqual("FunctionBDefaultChoice", retrievedAppliancee.CurrentSetting["lightFunctionB"]);
			Assert.AreEqual(1, retrievedAppliancee.Zone.Id, "Invalid Zone Id");
		}

		[Test]
		public void Appliance_4_PatchAppliance_SpecifyModelAndSettingsManually()
		{
			Appliance applianceToPatch = new Appliance
			{
				Id = 2,
				Model = new ApplianceModel { Id = 3 },
				CurrentSetting = new Dictionary<string, string>
				{
					["SwitchFunctionA"] = "FunctionAChoice2",
					["SwitchFunctionB"] = "FunctionBChoice2"
				}
			};

			Uri valuesUri = new Uri(_server.BaseAddress, relativeUri);

			// Patch and check returned result
			HttpRequestMessage request = HttpHelpers.CreateRequest<Appliance>(valuesUri, "application/json", new HttpMethod("PATCH"), applianceToPatch, new JsonMediaTypeFormatter());
			Appliance modifiedAppliance = HttpHelpers.PatchEntities<Appliance>(valuesUri, _server.ServerHandler, applianceToPatch, true, HttpStatusCode.Created);

			Assert.AreEqual(3, modifiedAppliance.Model.Id, "Invalid Model Id, test might have been ran in wrong order");
			Assert.AreEqual("FunctionAChoice2", modifiedAppliance.CurrentSetting["switchFunctionA"]);    //todo for some reason first letter gets lowercased...
			Assert.AreEqual("FunctionBChoice2", modifiedAppliance.CurrentSetting["switchFunctionB"]);

			// Get modified Appliance and make sure patch has been applied
			valuesUri = new Uri(_server.BaseAddress, relativeUri + "?ids=2");
			Appliance retrievedAppliancee = HttpHelpers.GetEntities<Appliance>(valuesUri, _server.ServerHandler).SingleOrDefault();

			Assert.IsNotNull(retrievedAppliancee);
			Assert.AreEqual(3, retrievedAppliancee.Model.Id, "Invalid Model Id, test might have been ran in wrong order");
			Assert.AreEqual("FunctionAChoice2", retrievedAppliancee.CurrentSetting["switchFunctionA"]);    //todo for some reason first letter gets lowercased...
			Assert.AreEqual("FunctionBChoice2", retrievedAppliancee.CurrentSetting["switchFunctionB"]);
		}

		[Test]
		public void Appliance_5_PatchAppliance_SpecifySettingsManuallyAndNoModel()
		{
			Appliance applianceToPatch = new Appliance
			{
				Id = 2,
				CurrentSetting = new Dictionary<string, string>
				{
					["SwitchFunctionA"] = "FunctionAChoice3",
					["SwitchFunctionB"] = "FunctionBChoice3"
				}
			};

			Uri valuesUri = new Uri(_server.BaseAddress, relativeUri);

			// Patch and check returned result
			HttpRequestMessage request = HttpHelpers.CreateRequest<Appliance>(valuesUri, "application/json", new HttpMethod("PATCH"), applianceToPatch, new JsonMediaTypeFormatter());
			Appliance modifiedAppliance = HttpHelpers.PatchEntities<Appliance>(valuesUri, _server.ServerHandler, applianceToPatch, true, HttpStatusCode.Created);

			Assert.AreEqual(3, modifiedAppliance.Model.Id, "Invalid Model Id, test might have been ran in wrong order");
			Assert.AreEqual("FunctionAChoice3", modifiedAppliance.CurrentSetting["switchFunctionA"]);    //todo for some reason first letter gets lowercased...
			Assert.AreEqual("FunctionBChoice3", modifiedAppliance.CurrentSetting["switchFunctionB"]);

			// Get modified Appliance and make sure patch has been applied
			valuesUri = new Uri(_server.BaseAddress, relativeUri + "?ids=2");
			Appliance retrievedAppliancee = HttpHelpers.GetEntities<Appliance>(valuesUri, _server.ServerHandler).SingleOrDefault();

			Assert.IsNotNull(retrievedAppliancee);
			Assert.AreEqual(3, retrievedAppliancee.Model.Id, "Invalid Model Id");
			Assert.AreEqual("FunctionAChoice3", retrievedAppliancee.CurrentSetting["switchFunctionA"]);    //todo for some reason first letter gets lowercased...
			Assert.AreEqual("FunctionBChoice3", retrievedAppliancee.CurrentSetting["switchFunctionB"]);
		}

		[Test]
		public void Appliance_5_PatchAppliance_SpecifyIncompleteSettingsManually_MustReturnBadRequest()
		{
			Appliance applianceToPatch = new Appliance
			{
				Id = 2,
				CurrentSetting = new Dictionary<string, string>
				{
					["SwitchFunctionA"] = "FunctionAChoice3",
					//["SwitchFunctionB"] = "FunctionBChoice3"
				}
			};

			Uri valuesUri = new Uri(_server.BaseAddress, relativeUri);

			// Patch and check returned result
			HttpRequestMessage request = HttpHelpers.CreateRequest<Appliance>(valuesUri, "application/json", new HttpMethod("PATCH"), applianceToPatch, new JsonMediaTypeFormatter());
			Appliance modifiedAppliance = HttpHelpers.PatchEntities<Appliance>(valuesUri, _server.ServerHandler, applianceToPatch, false, HttpStatusCode.InternalServerError);
			//todo should not be Error 500. Change controller/repo to follow best practices
			Assert.IsNull(modifiedAppliance);
		}

		[Test]
		public void Appliance_5_PatchAppliance_SpecifyWrongSettingsManually_MustReturnBadRequest()
		{
			Appliance applianceToPatch = new Appliance
			{
				Id = 2,
				CurrentSetting = new Dictionary<string, string>
				{
					["SwitchFunctionA"] = "FunctionAChoice3",
					["SwitchFunctionB"] = "WrongChoice"
				}
			};

			Uri valuesUri = new Uri(_server.BaseAddress, relativeUri);

			// Patch and check returned result
			HttpRequestMessage request = HttpHelpers.CreateRequest<Appliance>(valuesUri, "application/json", new HttpMethod("PATCH"), applianceToPatch, new JsonMediaTypeFormatter());
			Appliance modifiedAppliance = HttpHelpers.PatchEntities<Appliance>(valuesUri, _server.ServerHandler, applianceToPatch, false, HttpStatusCode.InternalServerError);
			//todo should not be Error 500. Change controller/repo to follow best practices
			Assert.IsNull(modifiedAppliance);
		}

		[Test]
		public void Appliance_6_DeleteAppliance()
		{
			int idOfApplianceToDelete = 1;

			// Make sure the appliance exists initially
			Uri valuesUri = new Uri(_server.BaseAddress, relativeUri + "?ids=" + idOfApplianceToDelete);
			Appliance existingAppliancee = HttpHelpers.GetEntities<Appliance>(valuesUri, _server.ServerHandler).SingleOrDefault();
			Assert.IsNotNull(existingAppliancee);

			// Delete it
			valuesUri = new Uri(_server.BaseAddress, relativeUri + "/" + idOfApplianceToDelete);
			Appliance deletedAppliance = HttpHelpers.DeleteEntity<Appliance>(valuesUri, _server.ServerHandler);

			Assert.AreEqual(idOfApplianceToDelete, deletedAppliance.Id);
			Assert.AreEqual("Appliance1", deletedAppliance.Name);
			Assert.AreEqual("Serge", deletedAppliance.UserName);

			// Make sure the appliance can't be retrieved again
			valuesUri = new Uri(_server.BaseAddress, relativeUri + "?ids=" + idOfApplianceToDelete);
			Appliance alreadyDeletedgAppliancee = HttpHelpers.GetEntities<Appliance>(valuesUri, _server.ServerHandler).SingleOrDefault();
			Assert.IsNull(alreadyDeletedgAppliancee);
		}
	}
}
