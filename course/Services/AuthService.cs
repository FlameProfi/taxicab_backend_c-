using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
using course.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace course.Services
{
    public interface IAuthService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
        string GenerateToken(User user);
        Task<User?> AuthenticateUser(string username, string password);
        Task<User?> RegisterUser(RegisterRequest request);
    }

    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly TaxiDbContext _context;

        public AuthService(IConfiguration configuration, TaxiDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "ExuAXvMZwkol6JIkJxXA2czryvm_Nx12!");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<User?> AuthenticateUser(string username, string password)
        {
            var user = await _context.Users 
                .FirstOrDefaultAsync(u => u.Username == username && u.IsActive);

            if (user == null || !VerifyPassword(password, user.PasswordHash))
            {
                return null;
            }

            return user;
        }

        public async Task<User?> RegisterUser(RegisterRequest request)
        {
            var existingUser = await _context.Users 
                .AnyAsync(u => u.Username == request.Username || u.Email == request.Email);

            if (existingUser)
            {
                throw new Exception("Пользователь с таким именем или email уже существует");
            }

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = HashPassword(request.Password),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Phone = request.Phone,
                Role = UserRole.Dispatcher, 
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user); 
            await _context.SaveChangesAsync();

            return user;
        }
    }
}