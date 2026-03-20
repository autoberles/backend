using autoberles_backend.Classes;
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

        [Authorize(Roles = "admin")]
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
                List<User> users;
                if (currentUserRole == Roles.Admin)
                    users = await context.Users.Include(x => x.Rentals).ThenInclude(x => x.Car).ToListAsync();
                else
                    return Forbid();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba a felhasználók lekérdezése során: {ex.Message}");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] int id)
        {
            try
            {
                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
                User? user;
                if (currentUserRole == Roles.Admin)
                    user = await context.Users.Include(x => x.Rentals).ThenInclude(x => x.Car).FirstOrDefaultAsync(x => x.Id == id);
                else
                    return Forbid();
                if (user == null)
                    return NotFound($"Nem található felhasználó a(z) {id} ID-val!");
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba a felhasználó lekérdezése során: {ex.Message}");
            }
        }

        [Authorize(Roles = "agent")]
        [HttpGet("customers")]
        public async Task<IActionResult> GetAllCustomers()
        {
            try
            {
                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
                List<User> customers;
                if (currentUserRole == Roles.Agent)
                    customers = await context.Users.Where(x => x.Role == Roles.Customer).Include(x => x.Rentals).ThenInclude(x => x.Car).ToListAsync();
                else
                    return Forbid();
                return Ok(customers);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba a felhasználók lekérdezése során: {ex.Message}");
            }
        }

        [Authorize(Roles = "agent")]
        [HttpGet("customers/{id}")]
        public async Task<IActionResult> GetCustomerById([FromRoute] int id)
        {
            try
            {
                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
                User? customer;
                if (currentUserRole == Roles.Agent)
                    customer = await context.Users.Where(x => x.Role == Roles.Customer).Include(x => x.Rentals).ThenInclude(x => x.Car).FirstOrDefaultAsync(x => x.Id == id);
                else
                    return Forbid();
                if (customer == null)
                    return NotFound($"Nem található felhasználó a(z) {id} ID-val!");
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba a felhasználó lekérdezése során: {ex.Message}");
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

        [Authorize(Roles = "admin,agent")]
        [HttpPost("users")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUser request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
                if (currentUserRole == "agent" && request.Role == "admin")
                    return BadRequest("Agent nem hozhat létre admint.");

                if (request.Password != request.ConfirmPassword)
                    return BadRequest("A jelszavak nem egyeznek.");

                if (await context.Users.AnyAsync(x => x.Email == request.Email))
                    return BadRequest("Ez az email már használatban van.");

                if (await context.Users.AnyAsync(x => x.PhoneNumber == request.PhoneNumber))
                    return BadRequest("Ez a telefonszám már használatban van.");

                var allowedRoles = new[] { "admin", "agent", "customer" };
                if (!allowedRoles.Contains(request.Role.ToLower()))
                    return BadRequest("Érvénytelen role.");

                var user = new User
                {
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PhoneNumber = request.PhoneNumber,
                    BirthDate = request.BirthDate,
                    Role = request.Role.ToLower(),
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
                };

                context.Users.Add(user);
                await context.SaveChangesAsync();
                return Ok("Felhasználó sikeresen létrehozva!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPatch("users/{id}")]
        public async Task<IActionResult> PatchUser(int id, [FromBody] JsonElement body)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

                var user = await context.Users.FindAsync(id);
                if (user == null)
                    return NotFound($"Nem található felhasználó a(z) {id} ID-val!");

                if (currentUserRole == Roles.Customer)
                    return StatusCode(403, "Customer nem módosíthat felhasználót.");
                if (body.ValueKind != JsonValueKind.Object)
                    return BadRequest("Érvénytelen JSON formátum!");

                foreach (var property in body.EnumerateObject())
                {
                    string propertyName = ConvertSnakeToPascal(property.Name);
                    var propInfo = typeof(User).GetProperty(propertyName,
                        BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (propInfo == null)
                        return BadRequest($"Ismeretlen mező: {property.Name}");

                    var newValue = JsonSerializer.Deserialize(property.Value.GetRawText(), propInfo.PropertyType);

                    if (propInfo.Name.Equals("PasswordHash", StringComparison.OrdinalIgnoreCase))
                        continue;
                    if (propInfo.Name.Equals("Role", StringComparison.OrdinalIgnoreCase))
                    {
                        string newRole = newValue?.ToString();

                        if (currentUserRole == Roles.Agent)
                            return StatusCode(403, "Agent nem módosíthatja a felhasználók szerepét.");

                        if (currentUserRole == Roles.Admin && user.Id == currentUserId)
                            return StatusCode(403, "Admin nem módosíthatja a saját szerepét.");
                    }

                    if (currentUserRole == Roles.Agent && user.Role != Roles.Customer)
                        return StatusCode(403, "Agent csak customereket módosíthat.");

                    if (propInfo.Name.Equals("Email", StringComparison.OrdinalIgnoreCase))
                    {
                        string emailStr = newValue?.ToString();
                        if (await context.Users.AnyAsync(x => x.Email == emailStr && x.Id != id))
                            return BadRequest("Ez az email cím már foglalt.");
                    }
                    propInfo.SetValue(user, newValue);
                }
                await context.SaveChangesAsync();
                return Ok($"A(z) {id} ID-val rendelkező felhasználó frissítésre került!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba a felhasználó frissítése során: {ex.Message}");
            }
        }

        [Authorize]
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (currentUserId == id)
                    return BadRequest("Saját fiók nem törölhető.");

                var userToDelete = await context.Users.FindAsync(id);
                if (userToDelete == null)
                    return NotFound($"Nem található felhasználó a(z) {id} ID-val!");

                if (currentUserRole == Roles.Agent)
                {
                    if (userToDelete.Role != Roles.Customer)
                        return BadRequest("Agent csak customert törölhet.");
                }
                else if (currentUserRole != Roles.Admin)
                    return Forbid();

                context.Users.Remove(userToDelete);
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
