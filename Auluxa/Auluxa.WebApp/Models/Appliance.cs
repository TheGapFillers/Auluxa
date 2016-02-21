using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System;

namespace Auluxa.WebApp.Models
{
	public class Appliance
	{
		public int Id { get; set; }
		[JsonIgnore]
		public string UserName { get; set; }
		public string Name { get; set; }
		public string ZoneName => Zone?.Name;

		[JsonIgnore]
		public Zone Zone { get; set; }

		public ApplianceModel Model { get; set; }

		public Dictionary<string, string> CurrentSetting	//seems can't perform add operation
		{
			get { return SerializedSetting != null ? JsonConvert.DeserializeObject<Dictionary<string, string>>(SerializedSetting) : null; }
			set { SerializedSetting = JsonConvert.SerializeObject(value); }
		}

		[JsonIgnore]
		public string SerializedSetting { get; set; }

		public void ApplyDefaultSettings()
		{
			if (Model?.PossibleSettings == null)
				throw new InvalidOperationException("Null or invalid ApplianceModel");

			var tempCurrentSetting = new Dictionary<string, string>();
			foreach (var kvp in Model.PossibleSettings)
			{
				tempCurrentSetting.Add(kvp.Key, kvp.Value[0]);
			}
			CurrentSetting = tempCurrentSetting;
		}

		public bool AreCurrentSettingsValid()
		{
			if (Model?.PossibleSettings == null)
				throw new InvalidOperationException("Null or invalid ApplianceModel");

			if (CurrentSetting.Count != Model.PossibleSettings.Count)
				return false;

			foreach(var kvp in CurrentSetting)
			{
				if(!Model.PossibleSettings.ContainsKey(kvp.Key))
					return false;

				if (!Model.PossibleSettings[kvp.Key].Contains(kvp.Value))
					return false;
			}

			return true;
		}
	}
}
