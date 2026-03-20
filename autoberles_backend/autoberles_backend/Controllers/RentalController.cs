using autoberles_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;

namespace autoberles_backend.Controllers
{
    [Route("api/")]
    [ApiController]
    public class RentalController : ControllerBase
    {
        CarRentalContext context = new CarRentalContext();


        [Authorize(Roles = "admin,agent")]
        [HttpGet("rentals")]
        public async Task<IActionResult> GetAllRentals()
        {
            List<Rental> rentals = await context.Rentals.ToListAsync();
            if (rentals == null)
                return BadRequest("Hiba a bérlések lekérdezése során");
            return Ok(rentals);
        }


        [Authorize(Roles = "admin,agent")]
        [HttpGet("rentals/{id}")]
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

        [Authorize]
        [HttpGet("rentals/my-rentals")]
        public async Task<IActionResult> GetMyRentals()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized("Nem sikerült azonosítani a felhasználót.");
                int userId = int.Parse(userIdClaim);
                var rentals = await context.Rentals.Where(x => x.UserId == userId && x.ReturnDate == null).ToListAsync();
                return Ok(rentals);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba a bérlések lekérdezése során: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("rentals/my-cars")]
        public async Task<IActionResult> GetMyRentedCars()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized("Nem sikerült azonosítani a felhasználót.");
                int userId = int.Parse(userIdClaim);

                var cars = await context.Rentals
                .Where(x => x.UserId == userId && x.EndDate >= DateTime.Today)
                .Include(x => x.Car).ThenInclude(x => x.AdditionalEquipment)
                .Select(x => x.Car).Distinct().ToListAsync();
                return Ok(cars);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPost("rental")]
        public async Task<IActionResult> PostRental([FromBody] Rental newRental)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized("Nem sikerült azonosítani a felhasználót.");
                int userId = int.Parse(userIdClaim);
                newRental.UserId = userId;

                if (newRental.CarId == 0)
                    return BadRequest("Az autó azonosítójának megadása kötelező.");
                if (!newRental.StartDate.HasValue || !newRental.EndDate.HasValue)
                    return BadRequest("A bérlés kezdetét és végét kötelező megadni.");

                var car = await context.Cars.FindAsync(newRental.CarId);
                if (car == null)
                    return BadRequest("A kiválasztott autó nem található.");

                bool isCarAlreadyRented = await context.Rentals.AnyAsync(x =>
                    x.CarId == newRental.CarId &&
                    x.StartDate <= newRental.EndDate &&
                    newRental.StartDate <= x.EndDate
                );
                if (isCarAlreadyRented)
                    return BadRequest("Ez az autó a megadott időszakban már ki van bérelve.");

                var validationResult = Rental.ValidateDates(newRental.EndDate.Value, new ValidationContext(newRental));
                if (validationResult != ValidationResult.Success)
                    return BadRequest(validationResult.ErrorMessage);

                int days = (newRental.EndDate.Value.Date - newRental.StartDate.Value.Date).Days + 1;
                if (days <= 0)
                    return BadRequest("A bérlés időtartama nem lehet 0 vagy negatív.");
                newRental.FullPrice = days * car.DefaultPricePerDay;
                newRental.ReturnDate = null;
                newRental.Damage = null;

                context.Rentals.Add(newRental);
                await context.SaveChangesAsync();
                return Ok(newRental);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba a bérlés létrehozása során: {ex.Message}");
            }
        }


        [Authorize(Roles = "admin,agent")]
        [HttpPatch("rentals/{id}")]
        public async Task<IActionResult> PatchRental(int id, [FromBody] JsonElement body)
        {
            try
            {
                var rental = await context.Rentals.FindAsync(id);
                if (rental == null)
                    return NotFound($"Nem található bérlés a(z) {id} ID-val!");

                if (body.ValueKind != JsonValueKind.Object)
                    return BadRequest("Érvénytelen JSON!");

                bool datesChanged = false;

                foreach (var property in body.EnumerateObject())
                {
                    string propertyName = ConvertSnakeToPascal(property.Name);

                    var propInfo = typeof(Rental).GetProperty(propertyName,
                        BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (propInfo == null)
                        return BadRequest($"Ismeretlen mező: {property.Name}");

                    var convertedValue = JsonSerializer.Deserialize(property.Value.GetRawText(), propInfo.PropertyType);

                    if (propInfo.Name == "StartDate" || propInfo.Name == "EndDate")
                        datesChanged = true;
                    propInfo.SetValue(rental, convertedValue);
                }

                if (datesChanged)
                {
                    if (rental.StartDate == null || rental.EndDate == null)
                        return BadRequest("StartDate és EndDate kötelező.");

                    int days = (rental.EndDate.Value.Date - rental.StartDate.Value.Date).Days + 1;
                    if (days <= 0)
                        return BadRequest("A bérlés időtartama nem lehet 0 vagy negatív.");

                    var car = await context.Cars.FindAsync(rental.CarId);
                    if (car == null)
                        return BadRequest("Nem található az autó.");

                    rental.FullPrice = days * car.DefaultPricePerDay;
                }
                await context.SaveChangesAsync();
                return Ok(rental);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba: {ex.Message}");
            }
        }


        [Authorize(Roles = "admin,agent")]
        [HttpDelete("rentals/{id}")]
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
