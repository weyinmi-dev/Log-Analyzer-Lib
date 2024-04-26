namespace Log_Analyzer_API.Data.DTOs
{
    public class LogDTO
    {
        public string Error { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string LogLevel { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string AdditionalInfo { get; set; } = string.Empty;
        public string LogFilePath { get; set; } = string.Empty;
        public string ErrorCode { get; set; } = string.Empty;
        public string LogId { get; set; } = string.Empty;
    }
}
