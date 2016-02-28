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
		[Test]
		public async Task ApplianceModel_GetTest()
		{
			var context = new TestDbContext();
			ApplianceModel am = BuildTestApplianceModel();
			context.ApplianceModels.Add(am);

			var applianceModelController = new ApplianceModelController(new EfApplicationRepository { Context = context });
			applianceModelController.Request = new System.Net.Http.HttpRequestMessage { RequestUri = new Uri("http://localhost:57776/api/models") };

			var result = await applianceModelController.Get() as OkNegotiatedContentResult<List<ApplianceModel>>;

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Content.Count);
			AssertApplianceModelsAreEqual(result.Content[0], am);
		}

		[Test]
		public async Task ApplianceModel_PostTest()
		{
			var applianceModelController = new ApplianceModelController(new EfApplicationRepository { Context = new TestDbContext()});
			applianceModelController.Request = new System.Net.Http.HttpRequestMessage { RequestUri = new Uri("http://localhost:57776/api/models") };

			ApplianceModel am = BuildTestApplianceModel();

			var result = await applianceModelController.Post(am) as CreatedNegotiatedContentResult<ApplianceModel>;

			Assert.IsNotNull(result);
			AssertApplianceModelsAreEqual(result.Content, am);
		}

		[Test]
		public async Task ApplianceModel_PatchTest()
		{
			var context = new TestDbContext();
			ApplianceModel am = BuildTestApplianceModel();
			context.ApplianceModels.Add(am);

			var applianceModelController = new ApplianceModelController(new EfApplicationRepository { Context = context });
			applianceModelController.Request = new System.Net.Http.HttpRequestMessage { RequestUri = new Uri("http://localhost:57776/api/models") };

			// Modify the appliance, send patch and check result
			//todo test null parameters
			am.Category = "SuperAC";
			am.BrandName = "Midea";
			am.ModelName = "AC3000";
			var resultPatch = await applianceModelController.Patch(am) as CreatedNegotiatedContentResult<ApplianceModel>;

			Assert.IsNotNull(resultPatch);
			AssertApplianceModelsAreEqual(resultPatch.Content, am);

			// Get all appliances must return modified appliance
			var resultGet = await applianceModelController.Get() as OkNegotiatedContentResult<List<ApplianceModel>>;

			Assert.IsNotNull(resultGet);
			Assert.AreEqual(1, resultGet.Content.Count);
			AssertApplianceModelsAreEqual(resultGet.Content[0], am);
		}

		[Test]
		public async Task ApplianceModel_DeleteTest()
		{
			var context = new TestDbContext();
			ApplianceModel am = BuildTestApplianceModel();
			context.ApplianceModels.Add(am);

			var applianceModelController = new ApplianceModelController(new EfApplicationRepository { Context = context });
			applianceModelController.Request = new System.Net.Http.HttpRequestMessage { RequestUri = new Uri("http://localhost:57776/api/models") };

			// Delete the appliance, send command and check result
			var resultDelete = await applianceModelController.Delete(am.Id) as OkNegotiatedContentResult<ApplianceModel>;

			Assert.IsNotNull(resultDelete);
			AssertApplianceModelsAreEqual(resultDelete.Content, am);

			// Get all appliances must return empty set
			var resultGet = await applianceModelController.Get() as OkNegotiatedContentResult<List<ApplianceModel>>;

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
