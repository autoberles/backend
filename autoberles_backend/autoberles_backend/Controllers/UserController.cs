using autoberles_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;

namespace autoberles_backend.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        CarRentalContext context = new CarRentalContext();

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await context.Users.Include(x => x.Rentals).ThenInclude(x => x.Car).ToListAsync();
            if (users == null)
                return BadRequest("Hiba a felhasználók lekérdezése során");
            return Ok(users);
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] int id)
        {
            try
            {
                var user = await context.Users.Include(x => x.Rentals)
                                              .ThenInclude(x => x.Car)
                                              .FirstOrDefaultAsync(x => x.Id == id);
                if (user == null)
                    return NotFound($"Nem található felhasználó a(z) {id} ID-val!");
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba {ex}");
            }
        }

        [Authorize]
        [HttpGet("users/me")]
        public async Task<IActionResult> GetMyProfile()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized("Nem sikerült azonosítani a felhasználót.");
                int userId = int.Parse(userIdClaim);

                var user = await context.Users.Where(x => x.Id == userId).Select(x => new
                    {
                        x.Id,
                        x.Email,
                        x.FirstName,
                        x.LastName,
                        x.PhoneNumber,
                        x.BirthDate,
                        x.Role
                    }).FirstOrDefaultAsync();

                if (user == null)
                    return NotFound("Felhasználó nem található.");
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba a felhasználó lekérdezése során: {ex.Message}");
            }
        }

        [HttpPost("user")]
        public async Task<IActionResult> PostUser([FromBody, Bind("FirstName,LastName,Email,BirthDate")] User user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                bool emailExists = await context.Users.AnyAsync(x => x.Email == user.Email);
                if (emailExists)
                    return BadRequest("Ez az email cím már létezik.");

                context.Users.Add(user);
                await context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba a felhasználó létrehozása során: {ex.Message}");
            }
        }

        [HttpPatch("users/{id}")]
        public async Task<IActionResult> PatchUser(int id, [FromBody] JsonElement body)
        {
            try
            {
                var user = await context.Users.FindAsync(id);
                if (user == null)
                    return NotFound($"Nem található felhasználó a(z) {id} ID-val!");

                if (body.ValueKind != JsonValueKind.Object)
                    return BadRequest("Érvénytelen JSON formátum!");

                foreach (var property in body.EnumerateObject())
                {
                    string propertyName = ConvertSnakeToPascal(property.Name);

                    var propInfo = typeof(User).GetProperty(propertyName,
                        BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (propInfo == null)
                        return BadRequest($"Ismeretlen mező: {property.Name}");

                    var convertedValue = JsonSerializer.Deserialize(property.Value.GetRawText(), propInfo.PropertyType);

                    if (propInfo.Name.Equals("Email", StringComparison.OrdinalIgnoreCase))
                    {
                        string newEmail = convertedValue?.ToString();
                        bool emailExists = await context.Users.AnyAsync(x => x.Email == newEmail && x.Id != id);

                        if (emailExists)
                            return BadRequest("Ez az email cím már foglalt.");
                    }
                    propInfo.SetValue(user, convertedValue);
                }
                await context.SaveChangesAsync();
                return Ok($"A(z) {id} ID-val rendelkező felhasználó frissítésre került!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba a felhasználó frissítése során: {ex.Message}");
            }
        }

        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                User? user = await context.Users.FindAsync(id);
                if (user == null)
                    return NotFound($"Nem található felhasználó a(z) {id} ID-val!");
                context.Users.Remove(user);
                await context.SaveChangesAsync();
                return Ok($"A(z) {id} ID-val rendelkező felhasználó sikeresen törölve!");
            }
            catch (DbUpdateException ex)
            {
                var message = ex.InnerException?.Message;
                return BadRequest(message ?? "Adatbázis hiba történt.");
            }
        }

        private string ConvertSnakeToPascal(string snakeCase)
        {
            return string.Join("", snakeCase.Split('_').Select(word => char.ToUpper(word[0]) + word.Substring(1)));
        }
    }
}
