using autoberles_backend.Classes;
using autoberles_backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json;

namespace autoberles_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        CarRentalContext context = new CarRentalContext();

        [HttpGet("user")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await context.Users.Include(x => x.Rentals).ThenInclude(x => x.Car).ToListAsync();
            if (users == null)
                return BadRequest("Hiba a felhasználók lekérdezése során");
            return Ok(users);
        }

        [HttpGet("{id}")]
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

        [HttpPost("user")]
        public async Task<IActionResult> PostUser([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                BirthDate = dto.BirthDate
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchUser(int id, [FromBody] JsonElement body)
        {
            var user = await context.Users.FindAsync(id);
            if (user == null)
                return NotFound($"Nem található felhasználó a(z) {id} ID-val!");
            if (body.ValueKind != JsonValueKind.Object)
                return BadRequest("Érvénytelen JSON formátum!");
            foreach (var property in body.EnumerateObject())
            {
                var propInfo = typeof(User).GetProperty(
                    property.Name,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (propInfo == null)
                    return BadRequest($"Ismeretlen mező: {property.Name}");

                var convertedValue = JsonSerializer.Deserialize(
                    property.Value.GetRawText(),
                    propInfo.PropertyType);

                propInfo.SetValue(user, convertedValue);
            }
            await context.SaveChangesAsync();
            return Ok($"A(z) {id} ID-val rendelkező felhasználó frissítésre került!");
        }

        [HttpDelete("{id}")]
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
            catch (Exception ex)
            {
                return BadRequest($"Hiba {ex}");
            }
        }
    }
}
