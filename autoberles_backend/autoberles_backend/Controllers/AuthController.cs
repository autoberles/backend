using autoberles_backend.Classes;
using autoberles_backend.Models;
using autoberles_backend.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;

namespace autoberles_backend.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly CarRentalContext _context;
        private readonly EmailService _emailService;

        public AuthController(CarRentalContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register register)
        {
            try
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
                    Role = Roles.Customer,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(register.Password)
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                _ = _emailService.SendEmailAsync(
                    user.Email,
                    "Sikeres regisztráció",
                    $"Kedves {user.FirstName}!<br><br>Sikeresen regisztráltál az Autoberelek autóbérlés oldalára!"
                );

                return Ok(new
                {
                    message = $"Sikeres regisztráció, email elküldve a(z) {user.Email} email címre!"
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba a regisztráció során: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == login.Email);

                if (user == null || !BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash))
                    return Unauthorized("Érvénytelen email vagy jelszó.");

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.GivenName, user.FirstName ?? user.Email),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var identity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                );

                var principal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    authProperties
                );

                return Ok(new
                {
                    user = new
                    {
                        id = user.Id,
                        firstName = user.FirstName,
                        email = user.Email,
                        role = user.Role
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba a bejelentkezés során: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                return Ok(new
                {
                    message = "Sikeres kijelentkezés."
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba a kijelentkezés során: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            try
            {
                if (User?.Identity?.IsAuthenticated != true)
                    return Unauthorized();

                return Ok(new
                {
                    id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                    firstName = User.FindFirst(ClaimTypes.GivenName)?.Value,
                    email = User.FindFirst(ClaimTypes.Email)?.Value,
                    role = User.FindFirst(ClaimTypes.Role)?.Value
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba a felhasználó lekérdezése során: {ex.Message}");
            }
        }

        [HttpPost("forgot_password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPassword forgotPassword)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == forgotPassword.Email);

                if (user == null)
                    return Ok("Ha létezik ilyen email, elküldtük a kódot.");

                var code = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
                var hungarianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
                var localNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, hungarianTimeZone);

                user.ResetTokenHash = TokenHelper.HashToken(code);
                user.ResetTokenExpiry = localNow.AddMinutes(10);

                await _context.SaveChangesAsync();

                var localExpiry = user.ResetTokenExpiry.Value;

                _ = _emailService.SendEmailAsync(
                    user.Email,
                    "Jelszó visszaállítás",
                    $@"<h2>Jelszó visszaállítás</h2>
                    <p>A kódod:</p>
                    <h1>{code}</h1>
                    <p>Eddig érvényes: {localExpiry:yyyy.MM.dd HH:mm}</p>"
                );

                return Ok("Ha létezik ilyen email, elküldtük a kódot.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba az elfelejtett jelszó művelet során: {ex.Message}");
            }
        }

        [HttpPost("reset_password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword resetPassword)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == resetPassword.Email);

                if (user == null || user.ResetTokenHash == null || user.ResetTokenExpiry == null)
                    return BadRequest("Érvénytelen vagy lejárt kód.");

                var hungarianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
                var localNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, hungarianTimeZone);

                if (user.ResetTokenExpiry < localNow)
                    return BadRequest("Érvénytelen vagy lejárt kód.");

                var hashedInput = TokenHelper.HashToken(resetPassword.Code);

                if (user.ResetTokenHash != hashedInput)
                    return BadRequest("Érvénytelen vagy lejárt kód.");

                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(resetPassword.NewPassword);
                user.ResetTokenHash = null;
                user.ResetTokenExpiry = null;

                await _context.SaveChangesAsync();

                return Ok("Jelszó sikeresen módosítva.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba a jelszó visszaállítása során: {ex.Message}");
            }
        }
    }
}