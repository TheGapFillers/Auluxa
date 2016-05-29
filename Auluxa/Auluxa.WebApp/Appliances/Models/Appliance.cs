﻿using System;
using System.Collections.Generic;
using System.Linq;
using Auluxa.WebApp.Zones.Models;
using Newtonsoft.Json;

namespace Auluxa.WebApp.Appliances.Models
{
    public class Appliance
    {
        public int Id { get; set; }

        /// <summary>
        /// Required: Appliance's owner
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Required: Appliance's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Appliance's Zone. When creating or editing Appliance, must give an existing Zone identified by its Id, all other Zone attributes will be ignored.
        /// </summary>
        public ICollection<Zone> Zones { get; set; }

        /// <summary>
        /// Required: Appliance's Model. When creating or editing Appliance, must give an existing ApplianceModel identified by its Id, all other ApplianceModel attributes will be ignored.
        /// </summary>
        public ApplianceModel Model { get; set; }

        /// <summary>
        /// Appliance's settings. If not set, ApplianceModel's default settings will be applied.
        /// Format: Dictionary[string, string] where Key is setting's name, and Value is setting's value.
        /// Warning: Key and Value must exist in ApplianceModel's PossibleSettings
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
                throw new InvalidOperationException("Null or invalid ApplianceModel");

            CurrentSetting = Model?.PossibleSettings?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value[0]);
        }

        public bool AreCurrentSettingsValid()
        {
            if (Model?.PossibleSettings == null)
                throw new InvalidOperationException("Null or invalid ApplianceModel");

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
