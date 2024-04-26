using Log_Analyzer_API.Data.DTOs;
using LogAnalyzerLibrary.Abstraction;
using LogAnalyzerLibrary.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Log_Analyzer_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class USController : ControllerBase
    {
        private readonly IUserStoriesServices _userStoriesServices;

        public USController(IUserStoriesServices userStoriesServices)
        {
            _userStoriesServices = userStoriesServices;
        }

        [HttpGet("searchLogInDirectory")]
        public ActionResult<IEnumerable<LogFileDTO>> SearchLogInDirectory(string searchedLog)
        {
            var logFiles = _userStoriesServices.SearchLogInDirectory(searchedLog);
            if (logFiles == null)
            {
                return NotFound();
            }

            // Convert the LogFile objects to LogFileDTOs
            var logFileDTOs = logFiles.Select(logFile => new LogFileDTO
            {
                LogFileName = logFile.LogFileName,
                LogFilePath = logFile.LogFilePath,
                LogFileSize = logFile.LogFileSize,
                DateCreated = logFile.DateCreated,
                NeedsToBeUploaded = logFile.NeedsToBeUploaded
            }).ToList();

            return Ok(logFileDTOs);
        }

        [HttpGet("numberOfUniqueErrors")]
        public ActionResult<int> NumberOfUniqueErrors([FromBody] LogDTO logDTO)
        {
            // Convert LogDTO to Log
            Log log = new Log
            {
                Error = logDTO.Error
            };

            int uniqueErrors = _userStoriesServices.NumberOfUniqueErrors(log);

            return Ok(uniqueErrors);
        }

        [HttpGet("numberOfDuplicatedErrors")]
        public ActionResult<int> NumberOfDuplicatedErrors([FromBody] LogDTO logDTO)
        {
            // Convert LogDTO to Log
            Log log = new Log
            {
                Error = logDTO.Error
            };

            int duplicatedErrors = _userStoriesServices.NumberOfDuplicatedErrors(log);

            return Ok(duplicatedErrors);
        }

        [HttpDelete("deleteArchive/{archiveFilePath}")]
        public async Task<ActionResult<LogFileDTO>> DeleteArchive(string archiveFilePath)
        {
            var deletedArchive = await _userStoriesServices.DeleteArchive(archiveFilePath);

            if (deletedArchive == null)
            {
                return NotFound();
            }

            // Convert the LogFile object to LogFileDTO
            var logFileDTO = new LogFileDTO
            {
                LogFileName = deletedArchive.LogFileName,
                LogFilePath = deletedArchive.LogFilePath,
                DateCreated = deletedArchive.DateCreated,
                LogFileSize = deletedArchive.LogFileSize,
                NeedsToBeUploaded = deletedArchive.NeedsToBeUploaded
            };

            return Ok(logFileDTO);
        }

        [HttpPost("archiveLogs")]
        public async Task<ActionResult<LogFileDTO>> ArchiveLogs([FromQuery] string startDate, [FromQuery] string endDate)
        {
            var archivedLog = await _userStoriesServices.ArchiveLogs(startDate, endDate);

            if (archivedLog == null)
            {
                return NotFound();
            }

            // Convert the LogFile object to LogFileDTO
            var logFileDTO = new LogFileDTO
            {
                LogFileName = archivedLog.LogFileName,
                LogFilePath = archivedLog.LogFilePath,
                DateCreated = archivedLog.DateCreated,
                LogFileSize = archivedLog.LogFileSize,
                NeedsToBeUploaded = archivedLog.NeedsToBeUploaded
            };

            return Ok(logFileDTO);
        }

        [HttpPost("uploadLogToRemoteServer")]
        public async Task<ActionResult<LogFileDTO>> UploadLogToRemoteServerPerAPI()
        {
            var uploadedLog = await _userStoriesServices.UploadLogToRemoteServerPerAPI();

            if (uploadedLog == null)
            {
                return NotFound();
            }

            // Convert the LogFile object to LogFileDTO
            var logFileDTO = new LogFileDTO
            {
                LogFileName = uploadedLog.LogFileName,
                LogFilePath = uploadedLog.LogFilePath,
                DateCreated = uploadedLog.DateCreated,
                LogFileSize = uploadedLog.LogFileSize,
                NeedsToBeUploaded = uploadedLog.NeedsToBeUploaded
            };

            return Ok(logFileDTO);
        }

        [HttpDelete("deleteLogs")]
        public async Task<ActionResult<LogFileDTO>> DeleteLogs(string archivedFilePath)
        {
            var deletedLog = await _userStoriesServices.DeleteLogs(archivedFilePath);

            if (deletedLog == null)
            {
                return NotFound();
            }

            // Convert the LogFile object to LogFileDTO
            var logFileDTO = new LogFileDTO
            {
                LogFileName = deletedLog.LogFileName,
                LogFilePath = deletedLog.LogFilePath,
                DateCreated = deletedLog.DateCreated,
                LogFileSize = deletedLog.LogFileSize,
                NeedsToBeUploaded = deletedLog.NeedsToBeUploaded
            };

            return Ok(logFileDTO);
        }

        [HttpGet("totalAvailableLogs")]
        public async Task<ActionResult<LogFileDTO>> TotalAvailableLogsInAPeriod([FromBody] LogDTO logDTO, [FromQuery] string startDateTime, [FromQuery] string endDateTime)
        {
            // Convert LogDTO to Log
            Log log = new Log
            {
                Error = logDTO.Error,
                LogFilePath = logDTO.LogFilePath
            };

            var logsInPeriod = await _userStoriesServices.TotalAvailableLogsInAPeriod(log, startDateTime, endDateTime);

            if (logsInPeriod == null)
            {
                return NotFound();
            }

            // Convert the LogFile object to LogFileDTO
            var logFileDTO = new LogFileDTO
            {
                LogFileName = logsInPeriod.LogFileName,
                LogFilePath = logsInPeriod.LogFilePath,
                DateCreated = logsInPeriod.DateCreated,
                LogFileSize = logsInPeriod.LogFileSize,
                NeedsToBeUploaded = logsInPeriod.NeedsToBeUploaded
            };

            return Ok(logFileDTO);
        }

        [HttpGet("searchLogsBySize/{logSize}")]
        public async Task<ActionResult<LogFileDTO>> SearchLogsPerSize(int logSize)
        {
            var logsBySize = await _userStoriesServices.SearchLogsPerSize(logSize);

            if (logsBySize == null)
            {
                return NotFound();
            }

            // Convert the LogFile object to LogFileDTO
            var logFileDTO = new LogFileDTO
            {
                LogFileName = logsBySize.LogFileName,
                LogFilePath = logsBySize.LogFilePath,
                DateCreated = logsBySize.DateCreated,
                LogFileSize = logsBySize.LogFileSize,
                NeedsToBeUploaded = logsBySize.NeedsToBeUploaded
            };

            return Ok(logFileDTO);
        }

        [HttpGet("searchLogsPerDirectory")]
        public ActionResult<IEnumerable<LogFileDTO>> SearchLogsPerDirectory([FromQuery] LogFile logFile)
        {
            var logs = _userStoriesServices.SearchLogsPerDirectory(logFile);

            if (logs == null)
            {
                return NotFound();
            }

            // Convert each LogFile object to LogFileDTO
            var logFileDTOs = logs.Select(logFile => new LogFileDTO
            {
                LogFileName = logFile.LogFileName,
                LogFilePath = logFile.LogFilePath,
                LogFileSize = logFile.LogFileSize,
                DateCreated = logFile.DateCreated,
                NeedsToBeUploaded = logFile.NeedsToBeUploaded
            }).ToList();

            return Ok(logFileDTOs);
        }
    }
}
