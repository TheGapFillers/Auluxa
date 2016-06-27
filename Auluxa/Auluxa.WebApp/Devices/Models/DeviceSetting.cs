using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Auluxa.WebApp.Devices.Models
{
	public class DeviceSetting
	{
		public int Id { get; set; }

		/// <summary>
		/// Required: Device these settings apply to
		/// Format: Dictionary [string, string[]] where Key is setting's name, and value is an array of possible values for this setting, the first one being the default one.
		/// (e.g. [['Fan', ['Off', 'Low', 'Medium', 'High']], ['AC', ['Off', 'Auto', 'Max']]])
		/// </summary>
		[JsonIgnore]
		public Device Device { get; set; }

		public int DeviceId => Device.Id;

		/// <summary>
		/// Required: Device's available settings. 
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
			if (Device?.Model?.PossibleSettings == null)
				throw new InvalidOperationException("Null or invalid Device or DeviceModel");

			if (Setting.Count != Device.Model.PossibleSettings.Count)
				return false;

			foreach (KeyValuePair<string, string> kvp in Setting)
			{
				if (!Device.Model.PossibleSettings.ContainsKey(kvp.Key))
					return false;

				if (!Device.Model.PossibleSettings[kvp.Key].Contains(kvp.Value))
					return false;
			}

			return true;
		}

	}
}
