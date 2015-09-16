using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auluxa.Models
{
	class LightSetting
	{
		public bool Lit { get; set; }
		public int IntensityPercent {
			get { return IntensityPercent; }
			set {
				IntensityPercent = Math.Max(Math.Min(value, 100), 0);
				if(IntensityPercent == 0) Lit = false;	//spec on slide 35
			}
		}
	}
}
