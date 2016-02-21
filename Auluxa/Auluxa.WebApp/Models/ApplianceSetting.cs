using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Auluxa.WebApp.Models
{
	public class ApplianceSetting
	{
		public int Id { get; set; }
		public string ApplianceName => Appliance?.Name;

		public Appliance Appliance { get; set; }

		public Dictionary<string, string> Setting
		{
			get { return SerializedSetting != null ? JsonConvert.DeserializeObject<Dictionary<string, string>>(SerializedSetting) : null; }
			set { SerializedSetting = JsonConvert.SerializeObject(value); }
		}

		[JsonIgnore]
		public string SerializedSetting { get; set; }

		public bool IsValid()
		{
			if (Appliance?.Model?.PossibleSettings == null)
				throw new InvalidOperationException("Null or invalid Appliance or ApplianceModel");

			if (Setting.Count != Appliance.Model.PossibleSettings.Count)
				return false;

			foreach (var kvp in Setting)
			{
				if (!Appliance.Model.PossibleSettings.ContainsKey(kvp.Key))
					return false;

				if (!Appliance.Model.PossibleSettings[kvp.Key].Contains(kvp.Value))
					return false;
			}

			return true;
		}

	}
}
