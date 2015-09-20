using System.Collections.Generic;

namespace Auluxa.Models
{
    public class Zone
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Appliance> Appliances { get; set; }
    }
}
