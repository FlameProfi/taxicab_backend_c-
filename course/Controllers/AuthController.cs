using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using course.DTO.Request;
using course.DTO.Response;
using course.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace course.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]

public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    
    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Post([FromBody] SignInRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
            return BadRequest(new ErrorResponse("Почта обязательна!"));

        if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length <= 8)
            return BadRequest(new ErrorResponse("Пароль обязательный и должен быть больше 8 символов!"));

        var user = _context.Users
            .ToList()
            .FirstOrDefault(x => x.Email == request.Email && x.Password == request.Password);
        if (user == null)
            return Unauthorized(new ErrorResponse("Отправлены неправильные данные"));
        
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };
        var token = new JwtSecurityToken(
            Constants.Iss,
            Constants.Aud,
            claims,
            null,
            DateTime.Now.AddDays(7),
            new SigningCredentials(Constants.SymmetricSecurityKey, SecurityAlgorithms.HmacSha256)
        );
        
        return Ok(new SignInResponse()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            User = new UserResponse()
            {
                Id = user.Id,
                Email = user.Email,
                Fullname = user.FullName,
            }
            
        });
    }
}