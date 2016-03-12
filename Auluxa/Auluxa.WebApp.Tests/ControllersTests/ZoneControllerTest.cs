using Auluxa.WebApp.Appliances.Models;
using Auluxa.WebApp.Appliances.Repositories;
using Auluxa.WebApp.Zones.Controllers;
using Auluxa.WebApp.Zones.Models;
using Auluxa.WebApp.Zones.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auluxa.WebApp.Tests.ControllersTests
{
	[TestFixture]
	public class ZoneControllerTest : BaseControllerTest<Zone, ZoneController>
	{
		[SetUp]
		public override void SetUp()
		{
			base.SetUp();
			Controller = new ZoneController(
				new EfZoneRepository { Context = Context },
				new EfApplianceRepository { Context = Context }
			);
			Controller.Request = new System.Net.Http.HttpRequestMessage { RequestUri = new Uri("http://localhost:57776/api/models") };
		}

		[TearDown]
		public override void TearDown()
		{
			base.TearDown();
		}

		[Test]
		public async Task ZoneController_GetTest()
		{
			await ModelController_GetTest(Context.Zones.Add, Controller.Get);
		}

		[TestCase("", 0)]
		[TestCase("0", 0)]
		[TestCase("1", 1)]
		[TestCase("1,2", 2)]
		[TestCase("2,3", 1)]
		public async Task ZoneController_GetByIdTest(string ids, int expectedCount)
		{
			await ModelController_GetIdTest(ids, expectedCount, Context.Zones.Add, Controller.Get);
		}

		[Test]
		public void ZoneController_GetByIdTest_InvalidFormatMustThrow()
		{
			ModelController_GetByIdTest_InvalidFormatMustThrow(Context.Zones.Add, Controller.Get);
		}

		[Test]
		public async Task ZoneController_PostTest()
		{
			await ModelController_PostTest(Controller.Post);
		}

		[Test]
		public async Task ZoneController_PatchTest()
		{
			Zone z = BuildTestModel();

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

			await ModelController_PatchTest(z, Context.Zones.Add, Controller.Get, Controller.Patch);
		}

		[Test]
		public async Task ZoneController_DeleteTest()
		{
			await ModelController_DeleteTest(z => z.Id, Context.Zones.Add, Controller.Get, Controller.Delete);
		}

		protected override Zone BuildTestModel(int id = 1)
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

		protected override void AssertModelsAreEqual(Zone expected, Zone actual)
		{
			Assert.AreEqual(expected.Name, actual.Name);
			Assert.AreEqual(expected.UserName, actual.UserName);
			//Assert.AreEqual(expected.Appliances, actual.Appliances);
		}
	}
}
