using LogAnalyzerLibrary.Entities;

namespace LogAnalyzerLibrary.Abstraction
{
    public interface IUserStoriesServices
    {
        IQueryable<Log> SearchLogInDirection(string searchedLog);
        IQueryable<Log> NumberOfUniqueErrors(string numberOfUniqueErrors);
        IQueryable<Log> NumberOfDuplicatedErrors(string numberOfDuplicatedErrors);
        Task<Log> DeleteArchive();
        Task<Log> ArchiveLogs();
        Task<Log> UploadLogToRemoteServerPerAPI();
        Task<Log> DeleteLogs();
        Task<Log> TotalAvailableLogsInAPeriod(Log log, string dateTime);
        Task<Log> SearchLogsPerSize(int logSize);
        IQueryable<Log> SearchLogsPerDirectory(Log log);
    }
}
