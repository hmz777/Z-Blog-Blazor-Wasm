﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMZSoftwareBlazorWebAssembly.Models
{
    public class GitRepo
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string[] Topics { get; set; }
        public int Stars { get; set; }
        public int Watchers { get; set; }
        public int Forks { get; set; }
    }
}
