﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalyzerLibrary.Entities
{
    public class Directory
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();
        public string DirectoryNamePath { get; set; } = string.Empty;
    }
}
