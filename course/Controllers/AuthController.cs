using course.Models;
using course.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace course.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly TaxiDbContext _context; 

        public AuthController(IAuthService authService, TaxiDbContext context)
        {
            _authService = authService;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest(new { message = "Имя пользователя и пароль обязательны" });
                }

                var user = await _authService.AuthenticateUser(request.Username, request.Password);

                if (user == null)
                {
                    return Unauthorized(new { message = "Неверное имя пользователя или пароль" });
                }

                var token = _authService.GenerateToken(user);

                var response = new LoginResponse
                {
                    Token = token,
                    User = new User
                    {
                        Id = user.Id,
                        Username = user.Username,
                        Email = user.Email,
                        Role = user.Role,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Phone = user.Phone,
                        IsActive = user.IsActive
                    },
                    ExpiresAt = DateTime.UtcNow.AddDays(7)
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ошибка при входе: {ex.Message}" });
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] RegisterRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _authService.RegisterUser(request);

                return CreatedAtAction(nameof(Register), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ошибка при регистрации: {ex.Message}" });
            }
        }

        [HttpGet("me")]
        public async Task<ActionResult<User>> GetCurrentUser()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var username = User.FindFirst(ClaimTypes.Name)?.Value;
                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                var role = User.FindFirst(ClaimTypes.Role)?.Value;

                if (userId == null)
                {
                    return Unauthorized(new { message = "Пользователь не авторизован" });
                }

                var user = await _context.Users.FindAsync(int.Parse(userId));
                if (user == null)
                {
                    return NotFound(new { message = "Пользователь не найден" });
                }

                var userInfo = new User
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role,
                    FirstName = user.FirstName ?? "",
                    LastName = user.LastName ?? "",
                    Phone = user.Phone,
                    IsActive = user.IsActive
                };

                return Ok(userInfo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ошибка при получении данных пользователя: {ex.Message}" });
            }
        }
    }
}