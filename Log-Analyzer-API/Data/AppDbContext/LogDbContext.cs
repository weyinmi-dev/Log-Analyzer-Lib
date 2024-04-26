using LogAnalyzerLibrary.Entities;
using Microsoft.EntityFrameworkCore;
using Directory = LogAnalyzerLibrary.Entities.Directory;


namespace Log_Analyzer_API.Data.AppDbContext
{
    public class LogDbContext : DbContext
    {
       public LogDbContext(DbContextOptions<LogDbContext> options) : base(options) 
        { 
            
        }

        public DbSet<Directory>? Directories { get; set; }
        public DbSet<LogFile>? LogFiles { get; set; }  
        public DbSet<Log>? Logs { get; set; }
        public DbSet<ArchivedLog>? ArchivedLogs { get; set;}
    }
}
