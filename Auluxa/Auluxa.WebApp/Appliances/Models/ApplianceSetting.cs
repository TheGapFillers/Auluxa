using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Auluxa.WebApp.Appliances.Models
{
	public class ApplianceSetting
	{
		public int Id { get; set; }
		public string ApplianceName => Appliance?.Name;

		/// <summary>
		/// Required: Appliance these settings apply to
		/// Format: Dictionary [string, string[]] where Key is setting's name, and value is an array of possible values for this setting, the first one being the default one.
		/// (e.g. [['Fan', ['Off', 'Low', 'Medium', 'High']], ['AC', ['Off', 'Auto', 'Max']]])
		/// </summary>
		public Appliance Appliance { get; set; }

		/// <summary>
		/// Required: Appliance's available settings. 
		/// Format: Dictionary [string, string[]] where Key is setting's name, and Value is an array of possible values for this setting, the first one being the default one.
		/// (e.g. [['Fan', ['Off', 'Low', 'Medium', 'High']], ['AC', ['Off', 'Auto', 'Max']]])
		/// </summary>
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
