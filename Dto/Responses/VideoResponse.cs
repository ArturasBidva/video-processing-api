namespace video_processing_api.Dto.Responses
{
    public class VideoResponse
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }
    }
}