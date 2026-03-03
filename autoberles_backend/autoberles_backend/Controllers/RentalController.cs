using autoberles_backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace autoberles_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalController : ControllerBase
    {
        CarRentalContext context = new CarRentalContext();

        [HttpGet("rental")]
        public async Task<IActionResult> GetAllRentals()
        {
            List<Rental> rentals = await context.Rentals.ToListAsync();
            if (rentals == null)
                return BadRequest("Hiba a bérlések lekérdezése során");
            return Ok(rentals);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRentalById([FromRoute] int id)
        {
            try
            {
                Rental? rental = await context.Rentals.FindAsync(id);
                if (rental == null)
                    return NotFound($"Nem található bérlés a(z) {id} ID-val!");
                return Ok(rental);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba {ex}");
            }
        }

        [HttpPost("rental")]
        public async Task<IActionResult> PostRental([FromBody] dynamic rental)
        {
            try
            {
                Rental newRental = JsonSerializer.Deserialize<Rental>(rental, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (newRental == null)
                    return BadRequest("Hiba az adatok konvertálása során!");
                context.Rentals.Add(newRental);
                context.SaveChanges();
                return Ok(newRental);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba {ex}");
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchRental(int id, [FromBody] dynamic rental)
        {
            try
            {
                Rental? existed = await context.Rentals.FindAsync(id);
                if (existed == null)
                    return NotFound($"Nem található bérlés a(z) {id} ID-val!");
                Rental? newRental = JsonSerializer.Deserialize<Rental>(rental, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                existed.CarId = newRental.CarId ?? existed.CarId;
                existed.UserId = newRental.UserId ?? existed.UserId;
                existed.StartDate = newRental.StartDate ?? existed.StartDate;
                existed.EndDate = newRental.EndDate ?? existed.EndDate;
                int rowAffected = await context.SaveChangesAsync();
                if (rowAffected == 0)
                    return BadRequest("Hiba a frissítés során!");
                return Ok($"A(z) {id} ID-val rendelkező bérlés frissítésre került!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba {ex}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRental(int id)
        {
            try
            {
                Rental? rental = await context.Rentals.FindAsync(id);
                if (rental == null)
                    return NotFound($"Nem található bérlés a(z) {id} ID-val!");
                context.Rentals.Remove(rental);
                await context.SaveChangesAsync();
                return Ok($"A(z) {id} ID-val rendelkező bérlés sikeresen törölve!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba {ex}");
            }
        }
    }
}
