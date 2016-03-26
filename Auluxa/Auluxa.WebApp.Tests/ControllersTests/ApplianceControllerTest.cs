using System;
using System.Threading.Tasks;
using Auluxa.WebApp.Appliances.Controllers;
using Auluxa.WebApp.Appliances.Models;
using Auluxa.WebApp.Appliances.Repositories;
using NUnit.Framework;
using System.Collections.Generic;
using Newtonsoft.Json;
using Auluxa.WebApp.Zones.Models;

namespace Auluxa.WebApp.Tests.ControllersTests
{
	[TestFixture]
	public class ApplianceControllerTest : BaseControllerTest<Appliance, ApplianceController>
	{
		[SetUp]
		public override void SetUp()
		{
			base.SetUp();
			Controller = new ApplianceController(
				new EfApplianceRepository { Context = Context, ZoneContext = Context }
			);
			Controller.Request = new System.Net.Http.HttpRequestMessage { RequestUri = new Uri(REQUEST_URI) };

			ContextAdd = Context.Appliances.Add;
			ControllerGet = Controller.Get;
			ControllerPost = Controller.Post;
			ControllerPatch = Controller.Patch;
			ControllerDelete = Controller.Delete;

			// Fill context with required mock data
			Context.ApplianceModels.Add(GetMockApplianceModel(1));
			Context.ApplianceModels.Add(GetMockApplianceModel(2));
			Context.Zones.Add(GetMockZone(1));
			Context.Zones.Add(GetMockZone(2));
		}

		[TearDown]
		public override void TearDown()
		{
			base.TearDown();
		}

		[Test]
		public async Task ApplianceController_GetTest()
		{
			await ModelController_GetTest();
		}

		[TestCase("", 0)]
		[TestCase("0", 0)]
		[TestCase("1", 1)]
		[TestCase("1,2", 2)]
		[TestCase("2,3", 1)]
		public async Task ApplianceController_GetByIdTest(string ids, int expectedCount)
		{
			await ModelController_GetIdTest(ids, expectedCount);
		}

		[Test]
		public void ApplianceController_GetByIdTest_InvalidFormatMustThrow()
		{
			ModelController_GetByIdTest_InvalidFormatMustThrow();
		}

		[Test]
		public async Task ApplianceController_PostTest()
		{
			await ModelController_PostTest();
		}

		[Test]
		public async Task ApplianceController_PatchTest()
		{
			Appliance a = BuildTestModel();

			// Modify the ApplianceModel, send patch and check result
			//todo test null parameters
			a.Name = "ApplianceUpdated";
			a.Model = GetMockApplianceModel(2);
			a.Zone = GetMockZone(2);
			a.CurrentSetting = new Dictionary<string, string>
			{
				["FunctionA"] = "FunctionADefaultChoice",
				["FunctionB"] = "FunctionBChoice2"
			};

			await ModelController_PatchTest(a);
		}

		[Test]
		public void ApplianceController_PatchTest_UseInvalidSettings_MustThrow()
		{
			Appliance a = BuildTestModel();

			// Modify the ApplianceModel, send patch and check result
			//todo test null parameters
			a.Name = "ApplianceUpdated";
			a.UserName = "Mr. Smith";
			a.Model = GetMockApplianceModel(2);
			a.CurrentSetting = new Dictionary<string, string>
			{
				["FunctionA"] = "InvalidSettingA",
				["FunctionB"] = "FunctionBChoice2"
			};

			var ex = Assert.ThrowsAsync<Exception>(async () => await ModelController_PatchTest(a));
			Assert.AreEqual(ex.Message, "Invalid settings, must follow appliance model");
		}

		[Test]
		public async Task ApplianceController_DeleteTest()
		{
			await ModelController_DeleteTest(am => am.Id);
		}

		protected override Appliance BuildTestModel(int id = 1)
		{
			return new Appliance
			{
				Id = id,
				Name = "Appliance1",
				UserName = "Mr. Bean",
				Zone = GetMockZone(1),
				Model = GetMockApplianceModel(1),
				CurrentSetting = new Dictionary<string, string>
				{
					["FunctionA"] = "FunctionAChoice2",
					["FunctionB"] = "FunctionBChoice3"
				}
				
			};
		}

		protected override void AssertModelsAreEqual(Appliance expected, Appliance actual)
		{
			Assert.AreEqual(expected.Name, actual.Name);
			Assert.AreEqual(expected.UserName, actual.UserName);
			Assert.AreEqual(JsonConvert.SerializeObject(expected.Model), JsonConvert.SerializeObject(actual.Model));
			Assert.AreEqual(JsonConvert.SerializeObject(expected.Zone), JsonConvert.SerializeObject(actual.Zone));
			Assert.AreEqual(expected.CurrentSetting, actual.CurrentSetting);
		}

		private ApplianceModel GetMockApplianceModel(int id)
		{
			if (id == 1)
				return new ApplianceModel()
				{
					Id = 1,
					Category = "AC",
					BrandName = "Dyson",
					ModelName = "AC2000",
					PossibleSettings = new Dictionary<string, string[]>
					{
						["FunctionA"] = new[] { "FunctionADefaultChoice", "FunctionAChoice2", "FunctionAChoice3" },
						["FunctionB"] = new[] { "FunctionBDefaultChoice", "FunctionBChoice2", "FunctionBChoice3" }
					}
				};

			return new ApplianceModel()
			{
				Id = 2,
				Category = "SuperAC",
				BrandName = "Midea",
				ModelName = "AC3000",
				PossibleSettings = new Dictionary<string, string[]>
				{
					["FunctionA"] = new[] { "FunctionADefaultChoice", "FunctionAChoice2", "FunctionAChoice3" },
					["FunctionB"] = new[] { "FunctionBDefaultChoice", "FunctionBChoice2", "FunctionBChoice3" }
				}
			};
		}

		private Zone GetMockZone(int id)
		{
			if (id == 1)
				return new Zone()
				{
					Id = 1,
					Name = "Bed Room",
					UserName = "Serge"
				};

			return new Zone()
			{
				Id = 2,
				Name = "Living Room",
				UserName = "Marcel"
			};
		}
	}
}
