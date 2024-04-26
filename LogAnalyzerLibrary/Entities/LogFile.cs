using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalyzerLibrary.Entities
{
    public class LogFile : Directory
    {
        public string LogFileName { get; set; } = string.Empty;
        public string LogFilePath { get; set;} = string.Empty;
        public int LogFileSize { get; set;}
        public DateTime DateCreated { get; set; }
        public bool NeedsToBeUploaded { get; set; }
    }
}
