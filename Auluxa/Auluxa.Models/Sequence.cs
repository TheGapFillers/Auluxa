using Newtonsoft.Json;
using System.Collections.Generic;

namespace Auluxa.Models
{
    public class Sequence
    {
        public int Id { get; set; }
        [JsonIgnore]
        public Scene Scene { get; set; }
        public List<Trigger> Triggers { get; set; }
    }
}
