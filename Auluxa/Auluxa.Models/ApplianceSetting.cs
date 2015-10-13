using Newtonsoft.Json;

namespace Auluxa.Models
{
	public class ApplianceSetting
	{
		public int ApplianceSettingId { get; set; }
		public string ApplianceName { get { return Appliance?.Name; } }
		[JsonIgnore]
		public Appliance Appliance { get; set; }
	}
}
