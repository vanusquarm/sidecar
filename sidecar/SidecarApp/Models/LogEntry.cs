namespace SidecarApp.Models
{
    public class LogEntry
    {
        public string SourceApplication { get; set; } = string.Empty;
        public string Severity { get; set; } = "Info";
        public string Message { get; set; } = string.Empty;
        public string RequestId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
