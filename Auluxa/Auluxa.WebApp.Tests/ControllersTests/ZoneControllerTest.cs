using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Auluxa.WebApp.Appliances.Models;
using Auluxa.WebApp.Appliances.Repositories;
using Auluxa.WebApp.Tests.Repositories;
using Auluxa.WebApp.Zones.Controllers;
using Auluxa.WebApp.Zones.Models;
using Auluxa.WebApp.Zones.Repositories;
using NUnit.Framework;

namespace Auluxa.WebApp.Tests.ControllersTests
{
	[TestFixture]
	public class ZoneControllerTest
	{
		public TestDbContext Context { get; set; }
		public ZoneController Controller { get; set; }

		[SetUp]
		public void SetUp()
		{
			Context = new TestDbContext();
			Controller = new ZoneController(new EfZoneRepository { Context = Context }, new EfApplianceRepository { Context = Context });
			Controller.Request = new System.Net.Http.HttpRequestMessage { RequestUri = new Uri("http://localhost:57776/api/models") };
		}

		[TearDown]
		public void TearDown()
		{
		    Controller?.Dispose();
		    Context?.Dispose();
		}

		[Test]
		public async Task ZoneController_GetTest()
		{
			Zone z = BuildTestZoneModel();
			Context.Zones.Add(z);

			var result = await Controller.Get() as OkNegotiatedContentResult<List<Zone>>;

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Content.Count);
			AssertZonesAreEqual(result.Content[0], z);
		}

		[TestCase("", 0)]
		[TestCase("0", 0)]
		[TestCase("1", 1)]
		[TestCase("1,2", 2)]
		[TestCase("2,3", 1)]
		public async Task ZoneController_GetByIdTest(string ids, int expectedCount)
		{
			Zone z1 = BuildTestZoneModel(1);
			Zone z2 = BuildTestZoneModel(2);
			Context.Zones.Add(z1);
			Context.Zones.Add(z2);

			var result = await Controller.Get(ids) as OkNegotiatedContentResult<List<Zone>>;

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedCount, result.Content.Count);
		}

		[Test]
		public void ZoneController_GetByIdTest_InvalidFormatMustThrow()
		{
			Zone z = BuildTestZoneModel();
			Context.Zones.Add(z);

			//var ex = Assert.ThrowsAsync<FormatException>(async () => await Controller.Get("haha")); //NIY
			Assert.That(async () => await Controller.Get("haha"), Throws.TypeOf<FormatException>());
		}

		[Test]
		public async Task ZoneController_PostTest()
		{
			Zone z = BuildTestZoneModel();

			var result = await Controller.Post(z) as CreatedNegotiatedContentResult<Zone>;

			Assert.IsNotNull(result);
			AssertZonesAreEqual(result.Content, z);
		}

		[Test]
		public async Task ZoneController_PatchTest()
		{
			Zone z = BuildTestZoneModel();
			Context.Zones.Add(z);

			// Modify the appliance, send patch and check result
			//todo test null parameters
			z.Name = "Bermuda Triangle";
			z.UserName = "Jack Sparrow";
			z.Appliances = new List<Appliance>
			{
				new Appliance
				{
					Id = 1,
					Name = "The Black Rock",
					Model = new ApplianceModel()
					{
						Id = 0,
						Category = "Sea Ship",
						BrandName = "Unknown",
						ModelName = "Black Rock"
					},
					CurrentSetting = new Dictionary<string, string>
					{
						["MaxSpeed"] = "60",
						["Cannons"] = "30"
					}
				},
			};
			var resultPatch = await Controller.Patch(z) as CreatedNegotiatedContentResult<Zone>;

			Assert.IsNotNull(resultPatch);
			AssertZonesAreEqual(resultPatch.Content, z);

			// Get all appliances must return modified appliance
			var resultGet = await Controller.Get() as OkNegotiatedContentResult<List<Zone>>;

			Assert.IsNotNull(resultGet);
			Assert.AreEqual(1, resultGet.Content.Count);
			AssertZonesAreEqual(resultGet.Content[0], z);
		}

		[Test]
		public async Task ZoneController_DeleteTest()
		{
			Zone z = BuildTestZoneModel();
			Context.Zones.Add(z);

			// Delete the appliance, send command and check result
			var resultDelete = await Controller.Delete(z.Id) as OkNegotiatedContentResult<Zone>;

			Assert.IsNotNull(resultDelete);
			AssertZonesAreEqual(resultDelete.Content, z);

			// Get all appliances must return empty set
			//todo should we make sure related appliances are not deleted in the process?
			var resultGet = await Controller.Get() as OkNegotiatedContentResult<List<Zone>>;

			Assert.IsNotNull(resultGet);
			Assert.AreEqual(0, resultGet.Content.Count);
		}

		private Zone BuildTestZoneModel(int id = 1)
		{
			return new Zone()
			{
				Id = id,
				Name = "Area 51",
				UserName = "Agent K",
				Appliances = new List<Appliance>
				{
					new Appliance
					{
						Id = 1,
						Name = "Millenium Falcon",
						Model = new ApplianceModel()
						{
							Id = 0,
							Category = "Space Ship",
							BrandName = "Corellian Engineering Corporation",
							ModelName = "YT-1300"
						},
						CurrentSetting = new Dictionary<string, string>
						{
							["HyperSpaceSpeed"] = "42",
							["CannonTowers"] = "2"
						}
					},
				}
			};
		}

		private void AssertZonesAreEqual(Zone expected, Zone actual)
		{
			Assert.AreEqual(expected.Name, actual.Name);
			Assert.AreEqual(expected.UserName, actual.UserName);
			Assert.AreEqual(expected.Appliances, actual.Appliances);
		}
	}
}
