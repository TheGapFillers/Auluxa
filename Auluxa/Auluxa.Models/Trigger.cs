using System;

namespace Auluxa.Models
{
    public class Trigger
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Appliance Appliance { get; set; }
        public TimeSpan Delay { get; set; }
    }
}
