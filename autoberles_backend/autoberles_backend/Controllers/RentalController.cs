using autoberles_backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
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
        public async Task<IActionResult> PostRental([FromBody] Rental newRental)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (newRental.CarId == 0 || newRental.UserId == 0)
                    return BadRequest("CarId és UserId megadása kötelező.");

                bool isCarAlreadyRented = await context.Rentals
                    .AnyAsync(x => x.CarId == newRental.CarId &&
                                  x.StartDate < newRental.EndDate &&
                                  newRental.StartDate < x.EndDate);

                if (isCarAlreadyRented)
                    return BadRequest("Ez az autó a megadott időszakban már ki van bérelve.");

                var validationResult = Rental.ValidateDates(newRental.EndDate!.Value, new ValidationContext(newRental));
                if (validationResult != ValidationResult.Success)
                    return BadRequest(validationResult!.ErrorMessage);

                context.Rentals.Add(newRental);
                await context.SaveChangesAsync();
                return Ok(newRental);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba a bérlés létrehozása során: {ex.Message}");
            }
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchRental(int id, [FromBody] Rental updatedRental)
        {
            try
            {
                var existed = await context.Rentals.FindAsync(id);
                if (existed == null)
                    return NotFound($"Nem található bérlés a(z) {id} ID-val!");
                if (updatedRental == null)
                    return BadRequest("Érvénytelen adatok!");

                int carIdToCheck = (updatedRental.CarId ?? existed.CarId)!.Value;
                DateTime startDateToCheck = updatedRental.StartDate ?? existed.StartDate!.Value;
                DateTime endDateToCheck = updatedRental.EndDate ?? existed.EndDate!.Value;

                bool isCarAlreadyRented = await context.Rentals.AnyAsync(x =>
                        x.Id != id &&
                        x.CarId == carIdToCheck &&
                        x.StartDate < endDateToCheck &&
                        startDateToCheck < x.EndDate
                    );
                if (isCarAlreadyRented)
                    return BadRequest("Ez az autó a megadott időszakban már ki van bérelve.");

                existed.CarId = updatedRental.CarId != 0 ? updatedRental.CarId : existed.CarId;

                existed.UserId = updatedRental.UserId != 0 ? updatedRental.UserId : existed.UserId;

                existed.StartDate = updatedRental.StartDate ?? existed.StartDate;
                existed.EndDate = updatedRental.EndDate ?? existed.EndDate;

                var validationResult = Rental.ValidateDates(existed.EndDate.Value, new ValidationContext(existed));

                if (validationResult != ValidationResult.Success)
                    return BadRequest(validationResult.ErrorMessage);

                int rowAffected = await context.SaveChangesAsync();
                if (rowAffected == 0)
                    return BadRequest("Hiba a frissítés során!");

                return Ok($"A(z) {id} ID-val rendelkező bérlés frissítésre került!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba a frissítés során: {ex.Message}");
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
