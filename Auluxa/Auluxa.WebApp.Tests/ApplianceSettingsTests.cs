using System;
using Auluxa.WebApp.Models;
using System.Collections.Generic;
using NUnit.Framework;

namespace Auluxa.WebApp.Tests
{
	[TestFixture]
	public class ApplianceSettingsTests
	{
		public ApplianceModel ApplianceModel { get; set; }
		public Appliance Appliance { get; set; }
		public ApplianceSetting ApplianceSetting { get; set; }

		[SetUp]
		public void SetUp()
		{
			ApplianceModel = new ApplianceModel("A/C", "BrandNameA", "A/C A")
			{
				Id = 1,
				PossibleSettings = new Dictionary<string, string[]>
				{
					["FunctionA"] = new[] { "FunctionADefaultChoice", "FunctionAChoice2", "FunctionAChoice3" },
					["FunctionB"] = new[] { "FunctionBDefaultChoice", "FunctionBChoice2", "FunctionBChoice3" }
				}
			};

			Appliance = new Appliance
			{
				Id = 1,
				Name = "Appliance1",
				Model = ApplianceModel,
			};

			ApplianceSetting = new ApplianceSetting
			{
				Appliance = Appliance,
			};
		}

		[Test]
		public void IsValidTest()
		{
			ApplianceSetting.Setting = new Dictionary<string, string>
			{
				["FunctionA"] = "FunctionAChoice2",
				["FunctionB"] = "FunctionBChoice3"
			};
			Assert.IsTrue(ApplianceSetting.IsValid());

			ApplianceSetting.Setting = new Dictionary<string, string>
			{
				["FunctionA"] = "FunctionAChoice2",
				["InvalidFunction"] = "FunctionBChoice3",
			};
			Assert.IsFalse(ApplianceSetting.IsValid());

			ApplianceSetting.Setting = new Dictionary<string, string>
			{
				["FunctionA"] = "FunctionAChoice2",
				["FunctionB"] = "InvalidChoice",
			};
			Assert.IsFalse(ApplianceSetting.IsValid());

			ApplianceSetting.Setting = new Dictionary<string, string>
			{
				["FunctionA"] = "FunctionAChoice2",
			};
			Assert.IsFalse(ApplianceSetting.IsValid());

			Appliance.Model = null;
			var ex = Assert.Throws(typeof(InvalidOperationException), () => ApplianceSetting.IsValid());
			Assert.AreEqual("Null or invalid Appliance or ApplianceModel", ex.Message);
		}
	}
}
