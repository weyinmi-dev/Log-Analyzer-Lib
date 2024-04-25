using LogAnalyzerLibrary;
using LogAnalyzerLibrary.Abstraction;
using LogAnalyzerLibrary.Entities;

namespace Log_Analyzer_API.Implementation
{
    public class UserStoriesServices : IUserStoriesServices
    {
        public Task<Log> ArchiveLogs()
        {
            throw new NotImplementedException();
        }

        public Task<Log> DeleteArchive()
        {
            throw new NotImplementedException();
        }

        public Task<Log> DeleteLogs()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Log> NumberOfDuplicatedErrors(string numberOfDuplicatedErrors)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Log> NumberOfUniqueErrors(string numberOfUniqueErrors)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Log> SearchLogInDirection(string searchedLog)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Log> SearchLogsPerDirectory(Log log)
        {
            throw new NotImplementedException();
        }

        public Task<Log> SearchLogsPerSize(int logSize)
        {
            throw new NotImplementedException();
        }

        public Task<Log> TotalAvailableLogsInAPeriod(Log log, string dateTime)
        {
            throw new NotImplementedException();
        }

        public Task<Log> UploadLogToRemoteServerPerAPI()
        {
            throw new NotImplementedException();
        }
    }
}
