using System.Collections.Generic;
using Auluxa.WebApp.Devices.Models;
using Newtonsoft.Json;

namespace Auluxa.WebApp.Scenes.Models
{
    public class Scene
    {
        public int Id { get; set; }
        [JsonIgnore]
        public string UserName { get; set; }
        public string Name { get; set; }

        public Sequence Sequence { get; set; }
        public Schedule Schedule { get; set; }

        public ICollection<DeviceSetting> DeviceSettings { get; set; }
    }
}
