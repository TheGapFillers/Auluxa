using System;
using System.Collections.Generic;
using System.Linq;
using Auluxa.WebApp.Zones.Models;
using Newtonsoft.Json;

namespace Auluxa.WebApp.Devices.Models
{
    public class Device
    {
        public int Id { get; set; }

        /// <summary>
        /// A collection of child devices
        /// </summary>
        public ICollection<Device> ChildDevices { get; set; }

        /// <summary>
        /// Required: Device's owner
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Required: Device's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Device's Zone. When creating or editing Device, must give an existing Zone identified by its Id, all other Zone attributes will be ignored.
        /// </summary>
        public ICollection<Zone> Zones { get; set; }

        /// <summary>
        /// Required: Device's Model. When creating or editing Device, must give an existing DeviceModel identified by its Id, all other DeviceModel attributes will be ignored.
        /// </summary>
        public DeviceModel Model { get; set; }

        /// <summary>
        /// Device's settings. If not set, DeviceModel's default settings will be applied.
        /// Format: Dictionary[string, string] where Key is setting's name, and Value is setting's value.
        /// Warning: Key and Value must exist in DeviceModel's PossibleSettings
        /// (e.g. [['Fan', 'Off'], ['AC', 'Auto']])
        /// </summary>
        public Dictionary<string, string> CurrentSetting	//todo Should not be exposed directly for get. All operations will fail.
        {
            get { return SerializedSetting != null ? JsonConvert.DeserializeObject<Dictionary<string, string>>(SerializedSetting) : null; }
            set { SerializedSetting = JsonConvert.SerializeObject(value); }
        }

        [JsonIgnore]
        public string SerializedSetting { get; set; }

        public void ApplyDefaultSettings()
        {
            if (Model?.PossibleSettings == null)
                throw new InvalidOperationException("Null or invalid DeviceModel");

            CurrentSetting = Model?.PossibleSettings?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value[0]);
        }

        public bool AreCurrentSettingsValid()
        {
            if (Model?.PossibleSettings == null)
                throw new InvalidOperationException("Null or invalid DeviceModel");

            if (CurrentSetting.Count != Model.PossibleSettings.Count)
                return false;

            foreach(var kvp in CurrentSetting)
            {
                if(!Model.PossibleSettings.ContainsKey(kvp.Key))
                    return false;

                if (!Model.PossibleSettings[kvp.Key].Contains(kvp.Value))
                    return false;
            }

            return true;
        }
    }
}
