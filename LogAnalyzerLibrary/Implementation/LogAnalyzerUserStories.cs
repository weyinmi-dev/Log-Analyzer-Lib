using LogAnalyzerLibrary.Abstraction;
using OnaxTools.Dto.Http;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalyzerLibrary.Implementation
{
    public class LogAnalyzerUserStories : ILogAnalyzerUserStories
    {
        private List<string> logDirectories;

        public async Task<GenResponse<bool>> InsertDirectories(List<string> model)
        {
            var Response = new GenResponse<bool>();
            try
            {
                logDirectories = new List<string>();
                foreach (var directory in model)
                {
                    string normalizedDirectory = directory.Replace('\\', '/');
                    logDirectories.Add(normalizedDirectory);
                }

                Response.Result = true;
                Response.IsSuccess = true;
                Response.Message = "Successfully loaded directory paths!";
                return Response;
            }
            catch (Exception ex)
            {
                return GenResponse<bool>.Failed("An Error happened!");
            }
        }
        public async Task<GenResponse<List<string>>> SearchLogs()
        {
            var Response = new GenResponse<List<string>>();

            try
            {
                List<string> logFiles = new List<string>();

                foreach (string directory in logDirectories)
                {
                    if (Directory.Exists(directory))
                    {
                        var files = Directory.GetFiles(directory, "*.log", SearchOption.AllDirectories);
                        logFiles.AddRange(files);
                    }
                }

                Response.IsSuccess = true;
                Response.Result = logFiles;
                return Response;
            }
            catch (Exception ex)
            {
                return GenResponse<List<string>>.Failed(
                    $"Sorry, there was an error processing your request. Error==>{ex.Message}");
            }
        }

        public async Task<GenResponse<long>> CountUniqueErrors()
        {
            var Response = new GenResponse<long>();
            try
            {
                var logFiles = await this.SearchLogs();
                List<string> errors = new List<string>();
                var count = 0;
                foreach (string logFile in logFiles.Result)
                {
                    using (StreamReader reader = new StreamReader(logFile))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string error = ExtractError(line);
                            if (!string.IsNullOrEmpty(error))
                            {
                                errors.Add((error));
                            }
                        }
                    }
                }

                count += errors.Distinct().Count();

                Response.IsSuccess = true;
                Response.Result = count;
                return Response;
            }
            catch (Exception ex)
            {
                return GenResponse<long>.Failed(
                    $"Sorry, there was an error processing your request. Error==>{ex.Message}");
            }

        }

        public async Task<GenResponse<long>> CountDuplicatedErrors()
        {
            var Response = new GenResponse<long>();
            try
            {
                var logFiles = await this.SearchLogs();
                List<string> errors = new List<string>();
                var count = 0;
                foreach (string logFile in logFiles.Result)
                {
                    using (StreamReader reader = new StreamReader(logFile))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string error = ExtractError(line);
                            if (!string.IsNullOrEmpty(error))
                            {
                                errors.Add((error));
                            }
                        }
                    }
                }
                count += errors.Distinct().Count();

                Response.IsSuccess = true;
                Response.Result = errors.Count() - count;
                return Response;
            }
            catch (Exception ex)
            {
                return GenResponse<long>.Failed(
                    $"Sorry, there was an error processing your request. Error==>{ex.Message}");
            }
        }

        public async Task<GenResponse<bool>> DeleteArchiveFromPeriod(DateTime startDate, DateTime endDate)
        {
            var Response = new GenResponse<bool>();
            try
            {
                int delCount = 0;
                string[] files = Directory.GetFiles(logDirectories.First(), "*.zip", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    DateTime creationDate = File.GetCreationTime(file);
                    if (creationDate >= startDate && creationDate <= endDate)
                    {
                        File.Delete(file);
                    }

                    delCount += 1;
                }
                Response.Result = delCount > 0;
                Response.IsSuccess = delCount > 0;
                Response.Message =
                    delCount > 0 ? "Successfully Deleted the Archives found" : "No archived file was found";
                return Response;
            }
            catch (Exception ex)
            {
                return GenResponse<bool>.Failed(
                    $"Sorry, there was an error processing your request. Error==>{ex.Message}");
            }

        }

        public async Task<GenResponse<bool>> ArchiveLogsFromPeriod(DateTime startDate, DateTime endDate)
        {

            var Response = new GenResponse<bool>();
            try
            {
                int archiveCount = 0;
                string targetDirectory = logDirectories.First();
                string zipFileName = $"{startDate.ToString("dd_MM_yyyy")}-{endDate.ToString("dd_MM_yyyy")}.zip";

                using (ZipArchive archive = ZipFile.Open(Path.Combine(targetDirectory, zipFileName), ZipArchiveMode.Create))
                {
                    foreach (string directory in logDirectories)
                    {
                        foreach (string file in Directory.GetFiles(directory, "*.log", SearchOption.AllDirectories))
                        {
                            DateTime creationDate = File.GetCreationTime(file);
                            if (creationDate >= startDate && creationDate <= endDate)
                            {
                                archive.CreateEntryFromFile(file, Path.GetFileName(file));
                                File.Delete(file);
                                archiveCount += 1;
                            }
                        }
                    }
                }
                Response.Result = archiveCount > 0;
                Response.IsSuccess = archiveCount > 0;
                Response.Message =
                    archiveCount > 0 ? "Successfully Archived files found" : "No file was found";
                return Response;
            }
            catch (Exception ex)
            {
                return GenResponse<bool>.Failed(
                    $"Sorry, there was an error processing your request. Error==>{ex.Message}");
            }

        }

        public async Task<GenResponse<bool>> UploadLogsToRemoteServer(string apiUrl, List<string> logs)
        {
            var Response = new GenResponse<bool>();
            try
            {
                using HttpClient client = new HttpClient();
                MultipartFormDataContent formData = new MultipartFormDataContent();
                var apiResponse = await client.PostAsync(apiUrl, formData);
                foreach (string log in logs)
                {
                    var fileContent = new StreamContent(File.OpenRead(log));
                    formData.Add(fileContent, "log", Path.GetFileName(log));

                    HttpResponseMessage response = apiResponse;
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Failed to upload {log}: {response.ReasonPhrase}");
                    }
                    else
                    {
                        Console.WriteLine("Successfully uploaded to remote server!");
                    }
                }

                Response.IsSuccess = true;
                Response.Result = apiResponse.IsSuccessStatusCode;
                return Response;
            }
            catch (Exception ex)
            {
                return GenResponse<bool>.Failed($"Sorry, upload to Remote Server failed. Error==>{ex.Message}");
            }


        }

        public async Task<GenResponse<bool>> DeleteLogsFromPeriod(DateTime startDate, DateTime endDate)
        {
            var Response = new GenResponse<bool>();
            try
            {
                int delCount = 0;
                List<string> selectedlogFiles = new List<string>();
                foreach (string directory in logDirectories)
                {
                    foreach (string file in Directory.GetFiles(directory, "*.log", SearchOption.AllDirectories))
                    {
                        DateTime creationDate = File.GetCreationTime(file);
                        if (creationDate >= startDate && creationDate <= endDate)
                        {
                            File.Delete(file);
                        }
                    }
                    delCount++;
                }
                Response.Result = delCount > 0;
                Response.IsSuccess = delCount > 0;
                Response.Message =
                    delCount > 0 ? "Successfully Deleted the Logs selected" : "No log was deleted";
                return Response;
            }
            catch (Exception ex)
            {
                return GenResponse<bool>.Failed(
                    $"Sorry, there was an error processing your request. Error==>{ex.Message}");
            }
        }

        public async Task<GenResponse<long>> CountTotalAvailableLogsInPeriod(DateTime startDate, DateTime endDate)
        {
            var Response = new GenResponse<long>();
            long totalLogs = 0;
            try
            {
                foreach (string directory in logDirectories)
                {
                    foreach (string file in Directory.GetFiles(directory, "*.log", SearchOption.AllDirectories))
                    {
                        DateTime creationDate = File.GetCreationTime(file);
                        if (creationDate >= startDate && creationDate <= endDate)
                        {
                            totalLogs++;
                        }
                    }
                }
                Response.IsSuccess = true;
                Response.Result = totalLogs;
                return Response;
            }
            catch (Exception ex)
            {
                return GenResponse<long>.Failed(
                    $"Sorry, there was an error processing your request. Error==>{ex.Message}");
            }

        }

        public async Task<GenResponse<List<string>>> SearchLogsPerSize(long minSizeKB, long maxSizeKB)
        {
            var Response = new GenResponse<List<string>>();
            try
            {
                List<string> logFilesInDirectory = new List<string>();
                List<string> logFilesInRange = new List<string>();
                foreach (string directory in logDirectories)
                {
                    foreach (string file in Directory.GetFiles(directory, "*.log", SearchOption.AllDirectories))
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        long fileSizeKB = fileInfo.Length / 1024; // Convert bytes to kilobytes
                        if (fileSizeKB >= minSizeKB && fileSizeKB <= maxSizeKB)
                        {
                            logFilesInRange.Add(file);
                        }
                    }
                }

                Response.IsSuccess = true;
                Response.Result = logFilesInRange;
                return Response;
            }
            catch (Exception ex)
            {
                return GenResponse<List<string>>.Failed(
                    $"Sorry, there was an error processing your request. Error==>{ex.Message}");
            }

        }

        public async Task<GenResponse<List<string>>> SearchLogsPerDirectory(string targetDirectory)
        {
            var response = new GenResponse<List<string>>();
            try
            {
                List<string> logFilesInDirectory = new List<string>();
                string normalizedDirectory = targetDirectory.Replace('\\', '/');

                if (logDirectories.Contains(normalizedDirectory))
                {
                    if (Directory.Exists(normalizedDirectory))
                    {                    
                        string[] logFiles = Directory.GetFiles(normalizedDirectory, "*.log", SearchOption.AllDirectories);
                        logFilesInDirectory.AddRange(logFiles);

                        response.IsSuccess = true;
                        response.Result = logFilesInDirectory;
                        response.Message = "Log files retrieved successfully.";
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "The specified directory does not exist.";
                    }
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "The specified directory has not been inputted in the list of directories.";
                }
                return response;
            }
            catch (Exception ex)
            {
                return GenResponse<List<string>>.Failed($"An error occurred while searching logs: {ex.Message}");
            }
        }

        private string ExtractError(string line)
        {
            string[] parts = line.Split(':');
            if (parts.Length >= 3)
            {
                return parts[2].Trim();
            }
            return null;
        }
    }
}
