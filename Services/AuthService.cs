using Microsoft.EntityFrameworkCore;
using video_processing_api.Data;
using video_processing_api.Dto.Requests;
using video_processing_api.Models;

namespace video_processing_api.Services
{
    public class AuthService(AppDbContext context, ValidationService validationService)
    {
        private readonly AppDbContext _context = context;
        private readonly ValidationService _validationService = validationService;

        public async Task<User> RegisterAsync(RegisterRequest request)
        {

            var validation = _validationService.ValidateRegistration(request);


            if (validation != ValidationResult.Valid)
            {

                throw new Exception(validation.ToString());
            }

            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
                throw new Exception("Username already exists");

            var user = MapToUser(request);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();


            return user;
        }
        private User MapToUser(RegisterRequest request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            return new User
            {
                //trim for safety
                Username = request.Username.Trim(),
                Email = request.Email.Trim().ToLower(),
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}

