namespace Auluxa.Models
{
	public enum FanSpeed 
	{
		Undefined, 
		Auto, 
		High, 
		Med, 
		Low
	}

	public enum AcMode 
	{
		Undefined, 
		Auto, 
		Heat, 
		Cool, 
		Eco
	}

	public class ClimateSetting : ApplianceSetting
	{
		public bool Enabled { get; set; }
		public int Temperature { get; set; }    //todo: manage unit: C/F
		public AcMode AcMode { get; set; }
		public FanSpeed FanSpeed { get; set; }
		public bool AcSwingActive { get; set; }

		//todo there is also a timer and some schedules for fan (see slide 29, 32, 33). Think about proper model
	}
}
