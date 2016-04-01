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
			Controller.Request = new System.Net.Http.HttpRequestMessage { RequestUri = new Uri(REQUEST_URI) };

			ContextAdd = Context.Zones.Add;
			ControllerGet = Controller.Get;
			ControllerPost = Controller.Post;
			ControllerPatch = Controller.Patch;
			ControllerDelete = Controller.Delete;
		}

		[TearDown]
		public override void TearDown()
		{
			base.TearDown();
		}

		[Test]
		public async Task ZoneController_Get()
		{
			await ModelController_GetAll();
		}

		[TestCase("", 0)]
		[TestCase("0", 0)]
		[TestCase("1", 1)]
		[TestCase("1,2", 2)]
		[TestCase("2,3", 1)]
		public async Task ZoneController_GetById(string ids, int expectedCount)
		{
			await ModelController_GetById(ids, expectedCount);
		}

		[Test]
		public void ZoneController_GetById_InvalidFormatMustThrow()
		{
			ModelController_GetById_InvalidFormatMustThrow();
		}

		[Test]
		public async Task ZoneController_Post()
		{
			await ModelController_Post();
		}

		[Test]
		public async Task ZoneController_Patch()
		{
			Zone z = BuildTestModel();

			// Modify the Zone, send patch and check result
			//todo test null parameters
			z.Name = "Bermuda Triangle";
			z.UserName = "Jack Sparrow";

			await ModelController_Patch(z);
		}

		[Test]
		public async Task ZoneController_Delete()
		{
			await ModelController_Delete(z => z.Id);
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
		}
	}
}
