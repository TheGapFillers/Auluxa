using System.Collections.Generic;
using Newtonsoft.Json;

namespace Auluxa.WebApp.Models
{
    public class ApplianceModel
    {
        public ApplianceModel()
        {            
        }

        public ApplianceModel(string category = null, string brandName = null, string modelName = null)
        {
            Category = category;
            BrandName = brandName;
            ModelName = modelName;
        }

        public string Category { get; set; }
        public int Id { get; set; }
        public string BrandName { get; set; }
        public string ModelName { get; set; }

        public Dictionary<string, string[]> PossibleSettings
        {
            get { return SerializedSettings != null ? JsonConvert.DeserializeObject<Dictionary<string, string[]>>(SerializedSettings) : null; }
            set { SerializedSettings = JsonConvert.SerializeObject(value); }
        }

        [JsonIgnore]
        public string SerializedSettings { get; set; }
    }
}
