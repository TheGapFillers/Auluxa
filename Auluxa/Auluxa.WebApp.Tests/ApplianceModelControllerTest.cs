using Auluxa.WebApp.Controllers;
using Auluxa.WebApp.Models;
using Auluxa.WebApp.Repositories;
using Auluxa.WebApp.Tests.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace Auluxa.WebApp.Tests
{
	[TestFixture]
	public class ApplianceModelControllerTest
	{
		[Test]
		public async Task GetApplianceModelTest()
		{
			var context = new TestDbContext();
			context.ApplianceModels.Add(BuildTestApplianceModel());

			var applianceModelController = new ApplianceModelController(new EfApplicationRepository { Context = context });
			applianceModelController.Request = new System.Net.Http.HttpRequestMessage { RequestUri = new Uri("http://localhost:57776/api/models") };

			var result = await applianceModelController.Get() as OkNegotiatedContentResult<ApplianceModel>;

			Assert.IsNotNull(result);
			//Assert.AreEqual(result.Content.Category, am.Category);
			//Assert.AreEqual(result.Content.BrandName, am.BrandName);
			//Assert.AreEqual(result.Content.ModelName, am.ModelName);
		}

		[Test]
		public async Task PostApplianceModelTest()
		{
			var applianceModelController = new ApplianceModelController(new EfApplicationRepository { Context = new TestDbContext()});
			applianceModelController.Request = new System.Net.Http.HttpRequestMessage { RequestUri = new Uri("http://localhost:57776/api/models") };

			ApplianceModel am = BuildTestApplianceModel();

			var result = await applianceModelController.Post(am) as CreatedNegotiatedContentResult<ApplianceModel>;

			Assert.IsNotNull(result);
			Assert.AreEqual(result.Content.Category, am.Category);
			Assert.AreEqual(result.Content.BrandName, am.BrandName);
			Assert.AreEqual(result.Content.ModelName, am.ModelName);
		}

		private ApplianceModel BuildTestApplianceModel()
		{
			return new ApplianceModel()
			{
				Id = 0, Category = "AC", BrandName = "Dyson", ModelName = "AC2000"
			};
		}
	}
}
