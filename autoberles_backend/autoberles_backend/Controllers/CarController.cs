using autoberles_backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace autoberles_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        CarRentalContext context = new CarRentalContext();

        [HttpGet("car")]
        public async Task<IActionResult> GetAllCars()
        {
            List<Car> cars = await context.Cars.ToListAsync();
            if (cars == null)
                return BadRequest("Hiba az autók lekérdezése során");
            return Ok(cars);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarById([FromRoute] int id)
        {
            try
            {
                Car? car = await context.Cars.FindAsync(id);
                if (car == null)
                    return NotFound($"Nem található autó a(z) {id} ID-val!");
                return Ok(car);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba {ex}");
            }
        }

        [HttpPost("car")]
        public async Task<IActionResult> PostCar([FromBody] dynamic car)
        {
            try
            {
                Car newCar = JsonSerializer.Deserialize<Car>(car, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (newCar == null)
                    return BadRequest("Hiba az adatok konvertálása során!");
                context.Cars.Add(newCar);
                context.SaveChanges();
                return Ok(newCar);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba {ex}");
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchCar(int id, [FromBody] dynamic car)
        {
            try
            {
                Car? existed = await context.Cars.FindAsync(id);
                if (existed == null)
                    return NotFound($"Nem található autó a(z) {id} ID-val!");
                Car? newCar = JsonSerializer.Deserialize<Car>(car, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                existed.Brand = newCar.Brand ?? existed.Brand;
                existed.Model = newCar.Model ?? existed.Model;
                existed.BranchId = newCar.BranchId ?? existed.BranchId;
                existed.Year = newCar.Year ?? existed.Year;
                existed.TransmissionId = newCar.TransmissionId ?? existed.TransmissionId;
                existed.FuelTypeId = newCar.FuelTypeId ?? existed.FuelTypeId;
                existed.NumberOfSeats = newCar.NumberOfSeats ?? existed.NumberOfSeats;
                existed.Price = newCar.Price ?? existed.Price;
                existed.LicensePlate = newCar.LicensePlate ?? existed.LicensePlate;
                int rowAffected = await context.SaveChangesAsync();
                if (rowAffected == 0)
                    return BadRequest("Hiba a frissítés során!");
                return Ok($"A(z) {id} ID-val rendelkező autó frissítésre került!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba {ex}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            try
            {
                Car? car = await context.Cars.FindAsync(id);
                if (car == null)
                    return NotFound($"Nem található autó a(z) {id} ID-val!");
                context.Cars.Remove(car);
                await context.SaveChangesAsync();
                return Ok($"A(z) {id} ID-val rendelkező autó sikeresen törölve!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba {ex}");
            }
        }
    }
}
