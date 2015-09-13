using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auluxa.Models
{
    public class Appliance
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }
        public Zone Zone { get; set; }
        public ApplianceSetting Setting { get; set; }
    }
}
