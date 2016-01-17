using System.Collections.Generic;
using Newtonsoft.Json;

namespace Auluxa.WebApp.Models
{
    public class Zone
    {
        public int Id { get; set; }
        [JsonIgnore]
        public string UserName { get; set; }
        public string Name { get; set; }
        public List<Appliance> Appliances { get; set; }
    }
}
