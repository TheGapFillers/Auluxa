using System;
using System.Collections.Generic;

namespace Auluxa.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public Scene Scene { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<DayOfWeek> DayOfWeeks { get; set; }
    }
}
