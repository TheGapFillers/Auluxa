using Newtonsoft.Json;

namespace Auluxa.Models
{
	public class Appliance
	{
		public int ApplianceId { get; set; }
		public string Name { get; set; }
		public string ZoneName { get { return Zone?.Name; } }
		[JsonIgnore]
		public Zone Zone { get; set; }
		public ApplianceSetting CurrentSetting { get; set; }
		public ApplianceModel Model { get; set; }
	}
}
