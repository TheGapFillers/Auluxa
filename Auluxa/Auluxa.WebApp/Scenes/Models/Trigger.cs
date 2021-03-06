﻿using System;
using Auluxa.WebApp.Devices.Models;

namespace Auluxa.WebApp.Scenes.Models
{
    public class Trigger
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Sequence Sequence { get; set; }
        public Device Device { get; set; }
        public TimeSpan Delay { get; set; }
    }
}
