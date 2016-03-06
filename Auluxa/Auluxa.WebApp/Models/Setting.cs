namespace Auluxa.WebApp.Models
{
	public class Setting
	{
		public int Id { get; set; } = 1;
		public string HoursFormat { get; set; } = "HH:mm";
		public string DateFormat { get; set; } = "dd-MM-yyyy";
		public string TimeZoneName { get; set; } = "UTC";
		public int? TimeZoneUtcOffset { get; set; } = 0;
	}
}
