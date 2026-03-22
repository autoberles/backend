using autoberles_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using autoberles_backend.Classes;
using autoberles_backend.Services;

namespace autoberles_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly CarRentalContext _context;
        private readonly IConfiguration _config;
        private readonly EmailService _emailService;

        public AuthController(CarRentalContext context, IConfiguration config, EmailService emailService)
        {
            _context = context;
            _config = config;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register register)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _context.Users.AnyAsync(x => x.Email == register.Email))
                return BadRequest("Ez az email már használatban van.");

            if (await _context.Users.AnyAsync(x => x.PhoneNumber == register.PhoneNumber))
                return BadRequest("Ez a telefonszám már használatban van.");

            var user = new User
            {
                Email = register.Email,
                FirstName = register.FirstName,
                LastName = register.LastName,
                PhoneNumber = register.PhoneNumber,
                BirthDate = register.BirthDate,
                Role = "customer",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(register.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            _ = _emailService.SendEmailAsync(
                user.Email,
                "Sikeres regisztráció",
                $"Kedves {user.FirstName}!<br><br>Sikeresen regisztráltál az Autokell autóbérlés oldalára!"
            ); 
            return Ok(new { message = $"Sikeres regisztráció, email elküldve a(z) {user.Email} email címre!" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == login.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash))
                return Unauthorized("Érvénytelen email vagy jelszó.");

            var token = GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}