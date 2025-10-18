namespace video_processing_api.Models
{
    public enum ValidationResult
    {
        Valid,
        UsernameTooShort,
        UsernameTooLong,
        PasswordTooShort,
        PasswordTooLong,
        PasswordMissingUppercase,
        PasswordMissingLowercase,
        PasswordMissingNumber,
        UsernameExists,
        EmailExists
    }
}
