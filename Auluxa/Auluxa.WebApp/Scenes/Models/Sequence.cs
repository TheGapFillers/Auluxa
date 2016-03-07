using System.Collections.Generic;
using Newtonsoft.Json;

namespace Auluxa.WebApp.Scenes.Models
{
    public class Sequence
    {
        public int Id { get; set; }
        [JsonIgnore]
        public Scene Scene { get; set; }
        public List<Trigger> Triggers { get; set; }
    }
}
