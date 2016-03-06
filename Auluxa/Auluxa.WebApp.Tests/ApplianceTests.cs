using System;
using Auluxa.WebApp.Models;
using System.Collections.Generic;
using NUnit.Framework;

namespace Auluxa.WebApp.Tests
{
	[TestFixture]
	public class ApplianceTests
	{
		public ApplianceModel ApplianceModel { get; set; }
		public Appliance Appliance { get; set; }

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
		}

		[Test]
		public void Appliance_ApplyDefaultSettingsTest()
		{
			Appliance.CurrentSetting = null;
			Appliance.ApplyDefaultSettings();

			Assert.IsTrue(Appliance.AreCurrentSettingsValid());
			Assert.AreEqual(Appliance.CurrentSetting["FunctionA"], "FunctionADefaultChoice");
			Assert.AreEqual(Appliance.CurrentSetting["FunctionB"], "FunctionBDefaultChoice");

			Appliance.Model = null;
			var ex = Assert.Throws(typeof(InvalidOperationException), () => Appliance.ApplyDefaultSettings());
			Assert.AreEqual("Null or invalid ApplianceModel", ex.Message);
		}

		[Test]
		public void Appliance_AreCurrentSettingsValidTest()
		{
			Appliance.CurrentSetting = new Dictionary<string, string>
			{
				["FunctionA"] = "FunctionAChoice2",
				["FunctionB"] = "FunctionBChoice3"
			};
			Assert.IsTrue(Appliance.AreCurrentSettingsValid());

			Appliance.CurrentSetting = new Dictionary<string, string>
			{
				["FunctionA"] = "FunctionAChoice2",
				["InvalidFunction"] = "FunctionBChoice3",
			};
			Assert.IsFalse(Appliance.AreCurrentSettingsValid());

			Appliance.CurrentSetting = new Dictionary<string, string>
			{
				["FunctionA"] = "FunctionAChoice2",
				["FunctionB"] = "InvalidChoice",
			};
			Assert.IsFalse(Appliance.AreCurrentSettingsValid());

			Appliance.CurrentSetting = new Dictionary<string, string>
			{
				["FunctionA"] = "FunctionAChoice2",
			};
			Assert.IsFalse(Appliance.AreCurrentSettingsValid());

			Appliance.Model = null;
			var ex = Assert.Throws(typeof(InvalidOperationException), () => Appliance.AreCurrentSettingsValid());
			Assert.AreEqual("Null or invalid ApplianceModel", ex.Message);
		}
	}
}
