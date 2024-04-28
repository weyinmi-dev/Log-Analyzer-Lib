using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnaxTools.Dto.Http;
using OnaxTools.Http;
using OnaxTools.Enums.Http;
namespace LogAnalyzerLibrary.Abstraction
{
    public interface ILogAnalyzerUserStories
    {
        Task<GenResponse<bool>> InsertDirectories(List<string> model);
        Task<GenResponse<List<string>>> SearchLogs();
        Task<GenResponse<long>> CountUniqueErrors();
        Task<GenResponse<long>> CountDuplicatedErrors();
        Task<GenResponse<bool>> DeleteArchiveFromPeriod(DateTime startDate, DateTime endDate);
        Task<GenResponse<bool>> ArchiveLogsFromPeriod(DateTime startDate, DateTime endDate);
        Task<GenResponse<bool>> UploadLogsToRemoteServer(string apiUrl, List<string> logs);
        Task<GenResponse<bool>> DeleteLogsFromPeriod(DateTime startDate, DateTime endDate);
        Task<GenResponse<long>> CountTotalAvailableLogsInPeriod(DateTime startDate, DateTime endDate);
        Task<GenResponse<List<string>>> SearchLogsPerSize(long minSizeKB, long maxSizeKB);
        Task<GenResponse<List<string>>> SearchLogsPerDirectory(string targetDirectory);
    }
}
