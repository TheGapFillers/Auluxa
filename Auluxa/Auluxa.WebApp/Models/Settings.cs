using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Auluxa.WebApp.Models
{
	public class Settings
	{
		public int Id { get; set; } = 1;
		public string HoursFormat { get; set; } = "HH:mm";
		public string DateFormat { get; set; } = "dd-MM-yyyy";
		public TimeZone TimeZone { get; set; }
	}
}
