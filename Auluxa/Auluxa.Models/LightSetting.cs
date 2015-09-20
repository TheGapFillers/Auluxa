using System;

namespace Auluxa.Models
{
	public class LightSetting : ApplianceSetting
	{
		public bool Lit { get; set; }

		private int _intensityPercent;
		public int IntensityPercent 
		{
			get
			{
				return _intensityPercent;
			}
			set 
			{
				_intensityPercent = Math.Max(Math.Min(value, 100), 0);
				if (IntensityPercent == 0) Lit = false;	//spec on slide 35
			}
		}
	}
}
