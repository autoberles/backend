using autoberles_backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            List<User> users = await context.Users.ToListAsync();
            if (users == null)
                return BadRequest("Hiba a felhasználók lekérdezése során");
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] int id)
        {
            try
            {
                User? user = await context.Users.FindAsync(id);
                if (user == null)
                    return NotFound($"Nem található felhasználó a {id} ID-val!");
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba {ex}");
            }
        }

        [HttpPost("user")]
        public async Task<IActionResult> PostUser([FromBody] dynamic user)
        {
            try
            {
                User newUser = JsonSerializer.Deserialize<User>(user, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (newUser == null)
                    return BadRequest("Hiba az adatok konvertálása során!");
                context.Users.Add(newUser);
                context.SaveChanges();
                return Ok(newUser);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba {ex}");
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchUser(int id, [FromBody] dynamic user)
        {
            try
            {
                User? existed = await context.Users.FindAsync(id);
                if (existed == null)
                    return NotFound($"Nem található felhasználó a {id} ID-val!");
                User? newUser = JsonSerializer.Deserialize<User>(user, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                existed.LastName = newUser.LastName ?? existed.LastName;
                existed.FirstName = newUser.FirstName ?? existed.FirstName;
                existed.Email = newUser.Email ?? existed.Email;
                existed.BirthDate = newUser.BirthDate ?? existed.BirthDate;
                int rowAffected = await context.SaveChangesAsync();
                if (rowAffected == 0)
                    return BadRequest("Hiba a frissítés során!");
                return Ok($"A {id} ID-val rendelkező felhasználó frissítésre került!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba {ex}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                User? user = await context.Users.FindAsync(id);
                if (user == null)
                    return NotFound($"Nem található felhasználó a {id} ID-val!");
                context.Users.Remove(user);
                await context.SaveChangesAsync();
                return Ok($"A {id} ID-val rendelkező felhasználó sikeresen törölve!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba {ex}");
            }
        }
    }
}
