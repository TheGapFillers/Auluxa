using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Auluxa.WebApp.Zones.Models
{
    public class CreateZoneViewModel
    {
        public string Name { get; set; }
    }

    public class UpdateZoneViewModel
    {
        public int zoneId { get; set; }
        public string Name { get; set; }
    }
}