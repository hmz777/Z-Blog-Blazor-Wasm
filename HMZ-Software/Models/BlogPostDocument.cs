﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMZSoftwareBlazorWebAssembly.Models
{
    public class BlogPostDocument
    {
        public string Markdown { get; set; }
        public string Html { get; set; }
        public YamlMetadata Yaml { get; set; }
    }

    public class YamlMetadata
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateUpdated { get; set; }
        public string[] Tags { get; set; }
    }
}
