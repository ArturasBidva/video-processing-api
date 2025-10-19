namespace video_processing_api.Models
{
    public enum VideoStatus
    {
        Uploaded,
        QueuedForProcessing,
        Processing,
        Completed,
        Failed
    }
    public class Video
    {

        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public VideoStatus Status { get; set; } = VideoStatus.Uploaded;
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
