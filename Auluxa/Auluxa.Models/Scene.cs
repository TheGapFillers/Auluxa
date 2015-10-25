using System.Collections.Generic;
using Newtonsoft.Json;

namespace Auluxa.Models
{
	public class Scene
	{
		public int Id { get; set; }
		[JsonIgnore]
		public string UserName { get; set; }
		public string Name { get; set; }
		public Sequence Sequence { get; set; }
		public Schedule Schedule { get; set; }
		public List<ApplianceSetting> ApplianceSettings { get; set; }
	}
}
