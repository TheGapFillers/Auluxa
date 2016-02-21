using System;
using System.Collections.Generic;
using System.Web.Http.Results;
using Newtonsoft.Json;

namespace Auluxa.WebApp.Models
{
	public class Zone
	{
		public int Id { get; set; }
		[JsonIgnore]
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
