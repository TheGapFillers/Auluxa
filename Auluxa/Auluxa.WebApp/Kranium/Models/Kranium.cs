namespace Auluxa.WebApp.Kranium.Models
{
	public class KraniumEntity
	{
		public int Id { get; set; } = 1;
		public string Name { get; set; }
		public string Version { get; set; }
		public string MacAddress { get; set; }
		public string IPAddress { get; set; }
		public string ZigBeePanId { get; set; }
		public string ZigBeeChannel { get; set; }
		public string ZigBeeMacAddress { get; set; }
	}
}
