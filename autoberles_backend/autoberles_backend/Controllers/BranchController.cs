using autoberles_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json;

namespace autoberles_backend.Controllers
{
    [Route("api/")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        CarRentalContext context = new CarRentalContext();

        [HttpGet("branches")]
        public async Task<IActionResult> GetAllBranches()
        {
            List<Branch> branches = await context.Branches.ToListAsync();
            if (branches == null)
                return BadRequest("Hiba a telephelyek lekérdezése során");
            return Ok(branches);
        }

        [HttpGet("branches/{id}")]
        public async Task<IActionResult> GetBranchById([FromRoute] int id)
        {
            try
            {
                Branch? branch = await context.Branches.FindAsync(id);
                if (branch == null)
                    return NotFound($"Nem található telephely a(z) {id} ID-val!");
                return Ok(branch);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba {ex}");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost("branch")]
        public async Task<IActionResult> PostBranch([FromBody] Branch branch)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (await context.Branches.AnyAsync(x => x.Address == branch.Address))
                    return BadRequest("Ez a cím már létezik.");

                if (await context.Branches.AnyAsync(x => x.Email == branch.Email))
                    return BadRequest("Ez az email már létezik.");

                if (await context.Branches.AnyAsync(x => x.PhoneNumber == branch.PhoneNumber))
                    return BadRequest("Ez a telefonszám már létezik.");

                context.Branches.Add(branch);
                await context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetBranchById), new { id = branch.Id }, branch);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba a telephely létrehozása során: {ex.Message}");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPatch("branches/{id}")]
        public async Task<IActionResult> PatchBranch(int id, [FromBody] JsonElement body)
        {
            try
            {
                var branch = await context.Branches.FindAsync(id);
                if (branch == null)
                    return NotFound($"Nem található telephely a(z) {id} ID-val!");
                if (body.ValueKind != JsonValueKind.Object)
                    return BadRequest("Érvénytelen JSON formátum!");

                foreach (var property in body.EnumerateObject())
                {
                    string propertyName = ConvertSnakeToPascal(property.Name);

                    var propInfo = typeof(Branch).GetProperty(property.Name,
                        BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (propInfo == null)
                        return BadRequest($"Ismeretlen mező: {property.Name}");

                    var convertedValue = JsonSerializer.Deserialize(property.Value.GetRawText(), propInfo.PropertyType);
                    string? newValue = convertedValue?.ToString();

                    if (propInfo.Name.Equals("City", StringComparison.OrdinalIgnoreCase) &&
                        await context.Branches.AnyAsync(x => x.City == newValue && x.Id != id))
                        return BadRequest("Ez a város már létezik.");

                    if (propInfo.Name.Equals("Address", StringComparison.OrdinalIgnoreCase) &&
                        await context.Branches.AnyAsync(x => x.Address == newValue && x.Id != id))
                        return BadRequest("Ez a cím már létezik.");

                    if (propInfo.Name.Equals("Email", StringComparison.OrdinalIgnoreCase) &&
                        await context.Branches.AnyAsync(x => x.Email == newValue && x.Id != id))
                        return BadRequest("Ez az email már létezik.");

                    if (propInfo.Name.Equals("PhoneNumber", StringComparison.OrdinalIgnoreCase) &&
                        await context.Branches.AnyAsync(x => x.PhoneNumber == newValue && x.Id != id))
                        return BadRequest("Ez a telefonszám már létezik.");
                    propInfo.SetValue(branch, convertedValue);
                }
                await context.SaveChangesAsync();
                return Ok($"A(z) {id} ID-val rendelkező telephely frissítésre került!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba a telephely frissítése során: {ex.Message}");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("branches/{id}")]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            try
            {
                Branch? branch = await context.Branches.FindAsync(id);
                if (branch == null)
                    return NotFound($"Nem található telephely a(z) {id} ID-val!");
                context.Branches.Remove(branch);
                await context.SaveChangesAsync();
                return Ok($"A(z) {id} ID-val rendelkező telephely sikeresen törölve!");
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
