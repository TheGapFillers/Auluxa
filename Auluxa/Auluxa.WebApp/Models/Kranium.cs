using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net;
using System.Net.NetworkInformation;

namespace Auluxa.WebApp.Models
{
	public class Kranium
	{
		public int Id { get; set; } = 1;
		public string Name { get; set; }
		public string Version { get; set; }
		public PhysicalAddress MacAddress { get; set; }
		public IPAddress IPAddress { get; set; }
		public string ZigBeePanId { get; set; }
		public string ZigBeeChannel { get; set; }
		public PhysicalAddress ZigBeeMacAddress { get; set; }
	}
}
