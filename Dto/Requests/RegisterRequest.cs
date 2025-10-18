using System.ComponentModel.DataAnnotations;

namespace video_processing_api.Dto.Requests
{
    public class RegisterRequest
    {
        [Required, MinLength(3), MaxLength(20)]
        public string Username { get; set; } = string.Empty;
        [Required,EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, MinLength(8), MaxLength(100) ,RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$")]
        public string Password { get; set; } = string.Empty;
    }
}
