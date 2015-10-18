using Newtonsoft.Json;
using System.Collections.Generic;

namespace Auluxa.Models
{
	public class ApplianceSetting
	{
		public int ApplianceSettingId { get; set; }
		public string ApplianceName { get { return Appliance?.Name; } }
		[JsonIgnore]
		public Appliance Appliance { get; set; }
		public Dictionary<string, string> CurrentSettings { get; set; }
	}
}
