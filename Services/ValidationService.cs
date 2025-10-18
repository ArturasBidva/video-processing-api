using System.Linq;
using video_processing_api.Dto.Requests;
using video_processing_api.Models;

namespace video_processing_api.Services
{
    public class ValidationService
    {
        public ValidationResult ValidateRegistration(RegisterRequest request)
        {
            if (request.Username.Length < 3) return ValidationResult.UsernameTooShort;
            if (request.Username.Length > 50) return ValidationResult.UsernameTooLong;
            if (request.Password.Length < 8) return ValidationResult.PasswordTooShort;
            if (request.Password.Length > 100) return ValidationResult.PasswordTooLong;
            if (!request.Password.Any(char.IsUpper)) return ValidationResult.PasswordMissingUppercase;
            if (!request.Password.Any(char.IsLower)) return ValidationResult.PasswordMissingLowercase;
            if (!request.Password.Any(char.IsDigit)) return ValidationResult.PasswordMissingNumber;

            return ValidationResult.Valid;
        }
    }
}
