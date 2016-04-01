using System.Collections.Generic;
using Auluxa.WebApp.Appliances.Models;
using Newtonsoft.Json;

namespace Auluxa.WebApp.Zones.Models
{
	public class Zone
	{
		public int Id { get; set; }
		public string UserName { get; set; }
		public string Name { get; set; }

		public List<Appliance> Appliances// { get; set; }
		{
			get { return _appliances ?? new List<Appliance>(); }
			set { _appliances = value; }
		}

		private List<Appliance> _appliances;
	}
}
