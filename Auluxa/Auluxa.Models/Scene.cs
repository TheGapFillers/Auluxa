using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auluxa.Models
{
    public class Scene
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Sequence Sequence { get; set; }
        public Schedule Schedule { get; set; }
        public List<ApplianceSetting> ApplianceSettings { get; set; }
    }
}
