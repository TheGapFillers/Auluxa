using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auluxa.Models
{
    public class Sequence
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Trigger> Triggers { get; set; }
    }
}
