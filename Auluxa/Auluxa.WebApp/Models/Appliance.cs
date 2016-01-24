using System.Collections.Generic;
using Newtonsoft.Json;

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
		public ApplianceSetting CurrentSetting { get; set; }

		//public Dictionary<string, string> CurrentSetting
		//{
		//	get { return SerializedSetting != null ? JsonConvert.DeserializeObject<Dictionary<string, string>>(SerializedSetting) : null; }
		//	set { SerializedSetting = JsonConvert.SerializeObject(value); }
		//}

		//[JsonIgnore]
		//public string SerializedSetting { get; set; }
	}
}
