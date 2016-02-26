﻿using Auluxa.WebApp.Controllers;
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
	public class ZoneControllerTest
	{
		[Test]
		public async Task Zone_GetTest()
		{
			var context = new TestDbContext();
			Zone z = BuildTestZoneModel();
			context.Zones.Add(z);

			var zonesController = new ZoneController(new EfApplicationRepository { Context = context });
			zonesController.Request = new System.Net.Http.HttpRequestMessage { RequestUri = new Uri("http://localhost:57776/api/models") };

			var result = await zonesController.Get() as OkNegotiatedContentResult<List<Zone>>;

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Content.Count);
			AssertZonesAreEqual(result.Content[0], z);
		}

		[Test]
		public async Task Zone_PostTest()
		{
			var zonesController = new ZoneController(new EfApplicationRepository { Context = new TestDbContext() });
			zonesController.Request = new System.Net.Http.HttpRequestMessage { RequestUri = new Uri("http://localhost:57776/api/models") };

			Zone z = BuildTestZoneModel();

			var result = await zonesController.Post(z) as CreatedNegotiatedContentResult<Zone>;

			Assert.IsNotNull(result);
			AssertZonesAreEqual(result.Content, z);
		}

		[Test]
		public async Task Zone_PatchTest()
		{
			var context = new TestDbContext();
			Zone z = BuildTestZoneModel();
			context.Zones.Add(z);

			var zonesController = new ZoneController(new EfApplicationRepository { Context = context });
			zonesController.Request = new System.Net.Http.HttpRequestMessage { RequestUri = new Uri("http://localhost:57776/api/models") };

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
			var resultPatch = await zonesController.Patch(z) as CreatedNegotiatedContentResult<Zone>;

			Assert.IsNotNull(resultPatch);
			AssertZonesAreEqual(resultPatch.Content, z);

			// Get all appliances must return modified appliance
			var resultGet = await zonesController.Get() as OkNegotiatedContentResult<List<Zone>>;

			Assert.IsNotNull(resultGet);
			Assert.AreEqual(1, resultGet.Content.Count);
			AssertZonesAreEqual(resultGet.Content[0], z);
		}

		[Test]
		public async Task ApplianceModel_DeleteTest()
		{
			var context = new TestDbContext();
			Zone z = BuildTestZoneModel();
			context.Zones.Add(z);

			var zonesController = new ZoneController(new EfApplicationRepository { Context = context });
			zonesController.Request = new System.Net.Http.HttpRequestMessage { RequestUri = new Uri("http://localhost:57776/api/models") };

			// Delete the appliance, send command and check result
			var resultDelete = await zonesController.Delete(z.Id) as OkNegotiatedContentResult<Zone>;

			Assert.IsNotNull(resultDelete);
			AssertZonesAreEqual(resultDelete.Content, z);

			// Get all appliances must return empty set
			//todo should we make sure related appliances are not deleted in the process?
			var resultGet = await zonesController.Get() as OkNegotiatedContentResult<List<Zone>>;

			Assert.IsNotNull(resultGet);
			Assert.AreEqual(0, resultGet.Content.Count);
		}

		private Zone BuildTestZoneModel()
		{
			return new Zone()
			{
				Id = 0,
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
