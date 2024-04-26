using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalyzerLibrary.Entities
{
    public class ArchivedLog : LogFile
    {
        public string archivedFilePath {  get; set; } = string.Empty;
    }
}
