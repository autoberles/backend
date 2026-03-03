using autoberles_backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace autoberles_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        CarRentalContext context = new CarRentalContext();

        [HttpGet("branch")]
        public async Task<IActionResult> GetAllBranches()
        {
            List<Branch> branches = await context.Branches.ToListAsync();
            if (branches == null)
                return BadRequest("Hiba a telephelyek lekérdezése során");
            return Ok(branches);
        }

        [HttpGet("{id}")]
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

        [HttpPost("branch")]
        public async Task<IActionResult> PostBranch([FromBody] dynamic branch)
        {
            try
            {
                Branch newBranch = JsonSerializer.Deserialize<Branch>(branch, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (newBranch == null)
                    return BadRequest("Hiba az adatok konvertálása során!");
                context.Branches.Add(newBranch);
                context.SaveChanges();
                return Ok(newBranch);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba {ex}");
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchBranch(int id, [FromBody] dynamic branch)
        {
            try
            {
                Branch? existed = await context.Branches.FindAsync(id);
                if (existed == null)
                    return NotFound($"Nem található telephely a(z) {id} ID-val!");
                Branch? newBranch = JsonSerializer.Deserialize<Branch>(branch, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                existed.City = newBranch.City ?? existed.City;
                existed.Address = newBranch.Address ?? existed.Address;
                existed.Email = newBranch.Email ?? existed.Email;
                existed.PhoneNumber = newBranch.PhoneNumber ?? existed.PhoneNumber;
                int rowAffected = await context.SaveChangesAsync();
                if (rowAffected == 0)
                    return BadRequest("Hiba a frissítés során!");
                return Ok($"A(z) {id} ID-val rendelkező telephely frissítésre került!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba {ex}");
            }
        }

        [HttpDelete("{id}")]
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
            catch (Exception ex)
            {
                return BadRequest($"Hiba {ex}");
            }
        }
    }
}
