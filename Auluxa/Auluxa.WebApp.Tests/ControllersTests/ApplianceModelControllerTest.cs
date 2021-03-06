﻿using System;
using System.Threading.Tasks;
using Auluxa.WebApp.Appliances.Controllers;
using Auluxa.WebApp.Appliances.Models;
using Auluxa.WebApp.Appliances.Repositories;
using NUnit.Framework;
using System.Collections.Generic;

namespace Auluxa.WebApp.Tests.ControllersTests
{
	[TestFixture]
	public class ApplianceModelControllerTest : BaseControllerTest<ApplianceModel, ApplianceModelController>
	{
		[SetUp]
		public override void SetUp()
		{
			base.SetUp();
			Controller = new ApplianceModelController(
				new EfApplianceRepository { Context = Context }
			);
			Controller.Request = new System.Net.Http.HttpRequestMessage { RequestUri = new Uri(REQUEST_URI) };

			ContextAdd = Context.ApplianceModels.Add;
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
		public async Task ApplianceModelController_Get()
		{
			await ModelController_GetAll();
		}

		[TestCase("", 0)]
		[TestCase("0", 0)]
		[TestCase("1", 1)]
		[TestCase("1,2", 2)]
		[TestCase("2,3", 1)]
		public async Task ApplianceModelController_GetById(string ids, int expectedCount)
		{
			await ModelController_GetById(ids, expectedCount);
		}

		[Test]
		public void ApplianceModelController_GetById_InvalidFormatMustThrow()
		{
			ModelController_GetById_InvalidFormatMustThrow();
		}

		[Test]
		public async Task ApplianceModelController_Post()
		{
			await ModelController_Post();
		}

		[Test]
		public async Task ApplianceModelController_Patch()
		{
			ApplianceModel am = BuildTestModel();

			// Modify the ApplianceModel, send patch and check result
			//todo test null parameters
			am.Category = "SuperAC";
			am.BrandName = "Midea";
			am.ModelName = "AC3000";
			am.PossibleSettings = new Dictionary<string, string[]>
			{
				["FunctionC"] = new[] { "FunctionCDefaultChoice", "FunctionCChoice2", "FunctionCChoice3" },
				["FunctionD"] = new[] { "FunctionDDefaultChoice", "FunctionDChoice2", "FunctionDChoice3" }
			};

			await ModelController_Patch(am);
		}

		[Test]
		public async Task ApplianceModelController_Delete()
		{
			await ModelController_Delete(am => am.Id);
		}

		protected override ApplianceModel BuildTestModel(int id = 1)
		{
			return new ApplianceModel()
			{
				Id = id, Category = "AC", BrandName = "Dyson", ModelName = "AC2000",
				PossibleSettings = new Dictionary<string, string[]>
				{
					["FunctionA"] = new[] { "FunctionADefaultChoice", "FunctionAChoice2", "FunctionAChoice3" },
					["FunctionB"] = new[] { "FunctionBDefaultChoice", "FunctionBChoice2", "FunctionBChoice3" }
				}
			};
		}

		protected override void AssertModelsAreEqual(ApplianceModel expected, ApplianceModel actual)
		{
			Assert.AreEqual(expected.Category, actual.Category);
			Assert.AreEqual(expected.BrandName, actual.BrandName);
			Assert.AreEqual(expected.ModelName, actual.ModelName);
			Assert.AreEqual(expected.PossibleSettings, actual.PossibleSettings);
		}
	}
}
