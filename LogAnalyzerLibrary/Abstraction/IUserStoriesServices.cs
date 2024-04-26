using LogAnalyzerLibrary.Entities;

namespace LogAnalyzerLibrary.Abstraction
{
    public interface IUserStoriesServices
    {
        IQueryable<LogFile> SearchLogInDirectory(string searchedLog);
        public int NumberOfUniqueErrors(Log log);
        public int NumberOfDuplicatedErrors(Log log);
        Task<LogFile> DeleteArchive(string archiveFilePath);
        Task<LogFile> ArchiveLogs(string startDate, string endDate);
        Task<LogFile> UploadLogToRemoteServerPerAPI();
        Task<LogFile> DeleteLogs(string archivedFilePath);
        Task<LogFile> TotalAvailableLogsInAPeriod(Log log, string startDateTime, string endDateTime);
        public Task<LogFile> SearchLogsPerSize(int logSize);
        IQueryable<LogFile> SearchLogsPerDirectory(LogFile logFile);
    }
}
