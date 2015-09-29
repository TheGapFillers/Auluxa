using Newtonsoft.Json;

namespace Auluxa.Models
{
    public class ApplianceSetting
    {
        public int ApplianceSettingId { get; set; }
        public Appliance Appliance { get; set; }
    }
}
