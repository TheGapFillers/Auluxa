using Auluxa.WebApp.Controllers;
using Auluxa.WebApp.Models;
using Auluxa.WebApp.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace Auluxa.WebApp.Tests
{
	[TestFixture]
	public class ApplianceModelControllerTest
	{
		public TestDbContext Context { get; set; }
		public ApplianceModelController Controller { get; set; }

		[SetUp]
		public void SetUp()
		{
			Context = new TestDbContext();
			Controller = new ApplianceModelController(new EfApplicationRepository { Context = Context });
			Controller.Request = new System.Net.Http.HttpRequestMessage { RequestUri = new Uri("http://localhost:57776/api/models") };
		}

		[TearDown]
		public void TearDown()
		{
			if (Controller != null) { Controller.Dispose(); }
			if (Context != null) { Context.Dispose(); }
		}

		[Test]
		public async Task ApplianceModelController_GetTest()
		{
			ApplianceModel am = BuildTestApplianceModel();
			Context.ApplianceModels.Add(am);

			var result = await Controller.Get() as OkNegotiatedContentResult<List<ApplianceModel>>;

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Content.Count);
			AssertApplianceModelsAreEqual(result.Content[0], am);
		}

		[Test]
		public async Task ApplianceModelController_PostTest()
		{
			ApplianceModel am = BuildTestApplianceModel();

			var result = await Controller.Post(am) as CreatedNegotiatedContentResult<ApplianceModel>;

			Assert.IsNotNull(result);
			AssertApplianceModelsAreEqual(result.Content, am);
		}

		[Test]
		public async Task ApplianceModelController_PatchTest()
		{
			ApplianceModel am = BuildTestApplianceModel();
			Context.ApplianceModels.Add(am);

			// Modify the appliance, send patch and check result
			//todo test null parameters
			am.Category = "SuperAC";
			am.BrandName = "Midea";
			am.ModelName = "AC3000";
			var resultPatch = await Controller.Patch(am) as CreatedNegotiatedContentResult<ApplianceModel>;

			Assert.IsNotNull(resultPatch);
			AssertApplianceModelsAreEqual(resultPatch.Content, am);

			// Get all appliances must return modified appliance
			var resultGet = await Controller.Get() as OkNegotiatedContentResult<List<ApplianceModel>>;

			Assert.IsNotNull(resultGet);
			Assert.AreEqual(1, resultGet.Content.Count);
			AssertApplianceModelsAreEqual(resultGet.Content[0], am);
		}

		[Test]
		public async Task ApplianceModelController_DeleteTest()
		{
			ApplianceModel am = BuildTestApplianceModel();
			Context.ApplianceModels.Add(am);

			// Delete the appliance, send command and check result
			var resultDelete = await Controller.Delete(am.Id) as OkNegotiatedContentResult<ApplianceModel>;

			Assert.IsNotNull(resultDelete);
			AssertApplianceModelsAreEqual(resultDelete.Content, am);

			// Get all appliances must return empty set
			var resultGet = await Controller.Get() as OkNegotiatedContentResult<List<ApplianceModel>>;

			Assert.IsNotNull(resultGet);
			Assert.AreEqual(0, resultGet.Content.Count);
		}

		private ApplianceModel BuildTestApplianceModel()
		{
			return new ApplianceModel()
			{
				Id = 0, Category = "AC", BrandName = "Dyson", ModelName = "AC2000"
			};
		}

		private void AssertApplianceModelsAreEqual(ApplianceModel expected, ApplianceModel actual)
		{
			Assert.AreEqual(expected.Category, actual.Category);
			Assert.AreEqual(expected.BrandName, actual.BrandName);
			Assert.AreEqual(expected.ModelName, actual.ModelName);
		}
	}
}
