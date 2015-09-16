using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auluxa.Models
{
	public enum FanSpeed {
		Undef, Auto, High, Med, Low
	}

	public enum ACMode {
		Undef, Auto, Heat, Cool, Eco
	}

	class ClimateSetting
	{
		public bool Enabled { get; set; }
		public int Temperature { get; set; }    //todo: manage unit: C/F
		public ACMode ACMode { get; set; }
		public FanSpeed FanSpeed { get; set; }
		public bool ACSwingActive { get; set; }

		//todo there is also a timer and some schedules for fan (see slide 29, 32, 33). Think about proper model
	}
}
