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

        [JsonIgnore]
        public List<Appliance> Appliances { get; set; } = new List<Appliance>();
    }
}
