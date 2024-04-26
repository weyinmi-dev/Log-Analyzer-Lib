using Log_Analyzer_API.Data.AppDbContext;
using LogAnalyzerLibrary;
using LogAnalyzerLibrary.Abstraction;
using LogAnalyzerLibrary.Entities;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;

namespace Log_Analyzer_API.Implementation
{
    public class UserStoriesServices : IUserStoriesServices
    {
        private readonly LogDbContext _context;

        public UserStoriesServices(LogDbContext context)
        {
            _context = context;
        }

        public IQueryable<LogFile> SearchLogInDirectory(string searchedLogFile)
        {
            return _context.LogFiles.Where(logFile => logFile.DirectoryNamePath.Contains(searchedLogFile));
        }

        public int NumberOfUniqueErrors(Log log)
        {
            // Implement logic to calculate the number of unique errors
            var uniqueErrors = _context.Logs.Where(l => l.Error == log.Error)
                                            .Distinct()
                                            .Count();
            return uniqueErrors;
        }

        public int NumberOfDuplicatedErrors(Log log)
        {
            // Implement logic to calculate the number of duplicated errors
            var duplicatedErrors = _context.Logs.Where(l => l.Error == log.Error)
                                                .GroupBy(l => l.Error)
                                                .Where(g => g.Count() > 1)
                                                .Count();
            return duplicatedErrors;
        }

        public async Task<LogFile> DeleteArchive(string archiveFilePath)
        {
            // Check if the archive file exists
            if (!File.Exists(archiveFilePath))
            {
                // If the file doesn't exist, return null or handle as necessary
                return null;
            }

            try
            {
                // Delete the archive file
                File.Delete(archiveFilePath);

                var archiveFileRecord = _context.ArchivedLogs.FirstOrDefault(a => a.archivedFilePath == archiveFilePath);
                if (archiveFileRecord != null)
                {
                    _context.ArchivedLogs.Remove(archiveFileRecord);
                    await _context.SaveChangesAsync();
                }

                // Return a LogFile object representing the deleted archive file
                var deletedArchiveFile = new LogFile
                {
                    LogFileName = Path.GetFileName(archiveFilePath),
                    LogFilePath = archiveFilePath,
                    DateCreated = DateTime.Now 
                };
                return deletedArchiveFile;
            }
            catch (Exception ex)
            {
                return null;
            }


        }
        public async Task<LogFile> ArchiveLogs(string startDate, string endDate)
        {
            // Parse start and end dates
            DateTime start = DateTime.Parse(startDate);
            DateTime end = DateTime.Parse(endDate);

            // Select log files within the specified date range
            var logFilesToArchive = _context.LogFiles.Where(logFile =>
                logFile.DateCreated >= start && logFile.DateCreated <= end).ToList();

            if (logFilesToArchive.Count == 0)
            {
                // No log files found in the specified date range
                return null;
            }

            // Create the zip file name based on the date range
            string zipFileName = $"{start.ToString("dd_MM_yyyy")}-{end.ToString("dd_MM_yyyy")}.zip";
            string zipFilePath = Path.Combine("", zipFileName);

            // Create the zip file and add the log files to it
            using (FileStream zipStream = new FileStream(zipFilePath, FileMode.Create))
            using (ZipArchive archive = new ZipArchive(zipStream, ZipArchiveMode.Create))
            {
                foreach (var logFile in logFilesToArchive)
                {
                    // Add each log file to the zip archive
                    ZipArchiveEntry entry = archive.CreateEntry(Path.GetFileName(logFile.LogFilePath));

                    using (StreamWriter writer = new StreamWriter(entry.Open()))
                    using (StreamReader reader = new StreamReader(logFile.LogFilePath))
                    {
                        string logContent = await reader.ReadToEndAsync();
                        await writer.WriteAsync(logContent);
                    }

                    // Delete the original log file
                    File.Delete(logFile.LogFilePath);
                }
            }

            // Add a new entry to ArchivedLog DbSet with the zip file path
            ArchivedLog archivedLog = new ArchivedLog
            {
                archivedFilePath = zipFilePath
            };
            _context.ArchivedLogs.Add(archivedLog);
            await _context.SaveChangesAsync();

            // Return the first archived log file as a sample result (or any desired value)
            return logFilesToArchive.FirstOrDefault();
        }

        public async Task<LogFile>UploadLogToRemoteServerPerAPI()
        {
            var logsToUpload = _context.LogFiles.Where(logFile => logFile.NeedsToBeUploaded).ToList();

            // Create an HTTP client
            using var httpClient = new HttpClient();

            // Define the remote server API endpoint
            var apiEndpoint = "https://weyinmisdummy-api.com/upload";

            // Convert the logs to JSON and send the HTTP POST request
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(apiEndpoint, logsToUpload);

            // Handle the response
            if (response.IsSuccessStatusCode)
            {
                // The upload was successful, mark the logs as uploaded in the database
                foreach (var logFile in logsToUpload)
                {
                    logFile.NeedsToBeUploaded = false;
                }

                await _context.SaveChangesAsync();

                // Return the first uploaded log file as a sample result
                return logsToUpload.FirstOrDefault();
            }
            else
            {
                throw new Exception("Failed to upload logs to the remote server.");
            }

        }

        public async Task<LogFile>DeleteLogs(string archivedFilePath)
        {
            // Implement logic to delete logs
            var logsToDelete = _context.LogFiles.Where(logFile => logFile.DateCreated < DateTime.Now.AddMonths(-1));
            _context.LogFiles.RemoveRange(logsToDelete);
            await _context.SaveChangesAsync();
            return logsToDelete.FirstOrDefault();
        }

        public async Task<LogFile>TotalAvailableLogsInAPeriod(Log log, string startDateTime, string endDateTime)
        {
            DateTime start = DateTime.Parse(startDateTime);
            DateTime end = DateTime.Parse(endDateTime);

            var logsInPeriod = _context.LogFiles.Where(logFile => logFile.DateCreated >= start && logFile.DateCreated <= end);
            return await logsInPeriod.FirstOrDefaultAsync();

        }

        public async Task<LogFile>SearchLogsPerSize(int logSize)
        {
            // Implement logic to search logs by size
            var logsBySize = _context.LogFiles.Where(logFile => logFile.LogFileSize == logSize);
            return await logsBySize.FirstOrDefaultAsync();

        }

        public IQueryable<LogFile>SearchLogsPerDirectory(LogFile logFile)
        {
            return _context.LogFiles.Where(l => l.LogFilePath == logFile.LogFilePath);
        }
    }
}
