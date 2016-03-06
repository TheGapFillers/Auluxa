using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Auluxa.WebApp.Scenes.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        [JsonIgnore]
        public Scene Scene { get; set; }
        public DateTime? ScheduleStartDate { get; set; }
        public DateTime? ScheduleEndDate { get; set; }

        public List<DayOfWeek> DayOfWeeks
        {
            get { return ScheduleDaysOfWeek?.Split(',').Cast<DayOfWeek>().ToList(); }
            set { ScheduleDaysOfWeek = string.Join(",", value.Cast<string>()); }
        }

        [JsonIgnore]
        public string ScheduleDaysOfWeek { get; set; }
    }
}
