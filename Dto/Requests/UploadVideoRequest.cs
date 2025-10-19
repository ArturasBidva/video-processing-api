using System.ComponentModel.DataAnnotations;
namespace video_processing_api.Dto.Requests
{

    public class UploadVideoRequest
    {
        [Required]
        [RegularExpression(@"^.*\.(mp4|avi|mov|wmv|flv|webm|mkv)$",
         ErrorMessage = "Only video files are allowed (mp4, avi, mov, wmv, flv, webm, mkv)")]
        [MaxLength(60)]
        public string FileName { get; set; } = string.Empty;
    }
}