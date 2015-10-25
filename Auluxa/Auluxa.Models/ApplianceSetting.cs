using Newtonsoft.Json;
using System.Collections.Generic;

namespace Auluxa.Models
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
	}
}
