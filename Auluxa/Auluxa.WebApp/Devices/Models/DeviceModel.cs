using System.Collections.Generic;
using Newtonsoft.Json;

namespace Auluxa.WebApp.Devices.Models
{
    public class DeviceModel
    {
        public DeviceModel()
        {            
        }

        public DeviceModel(string category = null, string brandName = null, string modelName = null)
        {
            Category = category;
            BrandName = brandName;
            ModelName = modelName;
        }

        public int Id { get; set; }

        /// <summary>
        /// Required: DeviceModel's category (e.g. "Air Conditionner", "Speaker"...)
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Required: DeviceModel's brand (e.g. "Philipps"...)
        /// </summary>
        public string BrandName { get; set; }

        /// <summary>
        /// Required: DeviceModel's name
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// Required: DeviceModel's available settings. 
        /// Format: Dictionary [string, string[]] where Key is setting's name, and Value is an array of possible values for this setting, the first one being the default one.
        /// (e.g. [['Fan', ['Off', 'Low', 'Medium', 'High']], ['AC', ['Off', 'Auto', 'Max']]])
        /// </summary>
        public Dictionary<string, string[]> PossibleSettings
        {
            get { return SerializedSettings != null ? JsonConvert.DeserializeObject<Dictionary<string, string[]>>(SerializedSettings) : null; }
            set { SerializedSettings = JsonConvert.SerializeObject(value); }
        }

        [JsonIgnore]
        public string SerializedSettings { get; set; }

        /// <summary>
        /// Required: DeviceModel's available functions. 
        /// Format: Dictionary [string, string[]] where Key is setting's name, and Value is an array of possible values for this setting, the first one being the default one.
        /// (e.g. [['Fan', ['Off', 'Low', 'Medium', 'High']], ['AC', ['Off', 'Auto', 'Max']]])
        /// </summary>
        public Dictionary<string, string[]> PossibleFunctions
        {
            get { return SerializedFunctions != null ? JsonConvert.DeserializeObject<Dictionary<string, string[]>>(SerializedFunctions) : null; }
            set { SerializedFunctions = JsonConvert.SerializeObject(value); }
        }

        [JsonIgnore]
        public string SerializedFunctions { get; set; }
    }
}
