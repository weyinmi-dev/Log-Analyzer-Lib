using LogAnalyzerLibrary.Abstraction;
using LogAnalyzerLibrary.Implementation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnaxTools.Dto.Http;

namespace Log_Analyzer_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogAnalyzerUSController : ControllerBase
    {
        private readonly ILogAnalyzerUserStories _logAnalyzerUserStoriesServices;

        public LogAnalyzerUSController(ILogAnalyzerUserStories logAnalyzerUserStoriesServices)
        {
            _logAnalyzerUserStoriesServices = logAnalyzerUserStoriesServices;
        }

        // Endpoint to insert directories
        [HttpPost("insert-directories")]
        public async Task<IActionResult> InsertDirectories([FromBody] List<string> directories)
        {
            var response = await _logAnalyzerUserStoriesServices.InsertDirectories(directories);
            return HandleResponse(response);
        }

        // Endpoint to search logs
        [HttpGet("search-logs")]
        public async Task<IActionResult> SearchLogs()
        {
            var response = await _logAnalyzerUserStoriesServices.SearchLogs();
            return HandleResponse(response);
        }

        // Endpoint to count unique errors
        [HttpGet("count-unique-errors")]
        public async Task<IActionResult> CountUniqueErrors()
        {
            var response = await _logAnalyzerUserStoriesServices.CountUniqueErrors();
            return HandleResponse(response);
        }

        // Endpoint to count duplicated errors
        [HttpGet("count-duplicated-errors")]
        public async Task<IActionResult> CountDuplicatedErrors()
        {
            var response = await _logAnalyzerUserStoriesServices.CountDuplicatedErrors();
            return HandleResponse(response);
        }

        // Endpoint to delete archive from a period
        [HttpDelete("delete-archive-from-period")]
        public async Task<IActionResult> DeleteArchiveFromPeriod([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var response = await _logAnalyzerUserStoriesServices.DeleteArchiveFromPeriod(startDate, endDate);
            return HandleResponse(response);
        }

        // Endpoint to archive logs from a period
        [HttpPost("archive-logs-from-period")]
        public async Task<IActionResult> ArchiveLogsFromPeriod([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var response = await _logAnalyzerUserStoriesServices.ArchiveLogsFromPeriod(startDate, endDate);
            return HandleResponse(response);
        }

        // Endpoint to upload logs to a remote server
        [HttpPost("upload-logs-to-remote-server")]
        public async Task<IActionResult> UploadLogsToRemoteServer([FromQuery] string apiUrl, [FromBody] List<string> logs)
        {
            var response = await _logAnalyzerUserStoriesServices.UploadLogsToRemoteServer(apiUrl, logs);
            return HandleResponse(response);
        }

        // Endpoint to delete logs from a period
        [HttpDelete("delete-logs-from-period")]
        public async Task<IActionResult> DeleteLogsFromPeriod([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var response = await _logAnalyzerUserStoriesServices.DeleteLogsFromPeriod(startDate, endDate);
            return HandleResponse(response);
        }

        // Endpoint to count total available logs in a period
        [HttpGet("count-total-available-logs-in-period")]
        public async Task<IActionResult> CountTotalAvailableLogsInPeriod([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var response = await _logAnalyzerUserStoriesServices.CountTotalAvailableLogsInPeriod(startDate, endDate);
            return HandleResponse(response);
        }

        // Endpoint to search logs per size
        [HttpGet("search-logs-per-size")]
        public async Task<IActionResult> SearchLogsPerSize([FromQuery] long minSizeKB, [FromQuery] long maxSizeKB)
        {
            var response = await _logAnalyzerUserStoriesServices.SearchLogsPerSize(minSizeKB, maxSizeKB);
            return HandleResponse(response);
        }

        // Endpoint to search logs per directory
        [HttpGet("search-logs-per-directory")]
        public async Task<IActionResult> SearchLogsPerDirectory(string targetDirectory)
        {
            var response = await _logAnalyzerUserStoriesServices.SearchLogsPerDirectory(targetDirectory);
            return HandleResponse(response);
        }

        // Helper method to handle the response from the service and return the appropriate HTTP response
        private IActionResult HandleResponse<T>(GenResponse<T> response)
        {
            if (response.IsSuccess)
            {
                return Ok(response.Result);
            }
            else
            {
                return StatusCode(500, response.Message);
            }
        }
    }
}
