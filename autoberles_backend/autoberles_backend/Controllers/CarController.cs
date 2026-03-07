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
    public class CarController : ControllerBase
    {
        CarRentalContext context = new CarRentalContext();


        [HttpGet("car")]
        public async Task<IActionResult> GetAllCars()
        {
            try
            {
                var cars = await context.Cars
                    .Include(x => x.AdditionalEquipment)
                        .ThenInclude(x => x.AirConditioning)
                    .Include(x => x.Branch)
                    .Include(x => x.FuelType)
                    .Include(x => x.TransmissionType)
                    .Include(x => x.Rentals)
                    .ToListAsync();
                return Ok(cars);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba az autók lekérdezése során: {ex.Message}");
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarById([FromRoute] int id)
        {
            try
            {
                var car = await context.Cars
                            .Include(x => x.AdditionalEquipment)
                                .ThenInclude(x => x.AirConditioning)
                            .Include(x => x.Branch)
                            .Include(x => x.FuelType)
                            .Include(x => x.TransmissionType)
                            .Include(x => x.Rentals)
                            .FirstOrDefaultAsync(x => x.Id == id); 
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
        public async Task<IActionResult> PostCar([FromBody] CreateCarDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var car = new Car
                {
                    Brand = dto.Brand,
                    Model = dto.Model,
                    Year = dto.Year,
                    NumberOfSeats = dto.NumberOfSeats,
                    Price = dto.Price,
                    LicensePlate = dto.LicensePlate,
                    BranchId = dto.BranchId,
                    TransmissionId = dto.TransmissionId,
                    FuelTypeId = dto.FuelTypeId
                };

                context.Cars.Add(car);
                await context.SaveChangesAsync();

                if (dto.AdditionalEquipment != null)
                {
                    var equipment = new AdditionalEquipment
                    {
                        CarId = car.Id,
                        ParkingSensors = dto.AdditionalEquipment.ParkingSensors,
                        HeatedSeats = dto.AdditionalEquipment.HeatedSeats,
                        Navigation = dto.AdditionalEquipment.Navigation,
                        LeatherSeats = dto.AdditionalEquipment.LeatherSeats,
                        AirConditioningId = dto.AdditionalEquipment.AirConditioningId
                    };

                    context.AdditionalEquipments.Add(equipment);
                    await context.SaveChangesAsync();
                }
                return CreatedAtAction(nameof(GetCarById), new { id = car.Id }, car);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba az autó létrehozása során: {ex.Message}");
            }
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchCar(int id, [FromBody] JsonElement body)
        {
            try
            {
                var car = await context.Cars.FindAsync(id);
                if (car == null)
                    return NotFound($"Nem található autó a(z) {id} ID-val!");
                if (body.ValueKind == JsonValueKind.Object)
                {
                    foreach (var property in body.EnumerateObject())
                    {
                        UpdateProperty(car, property.Name, property.Value);
                    }
                }
                else if (body.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in body.EnumerateArray())
                    {
                        var field = item.GetProperty("field").GetString();
                        var value = item.GetProperty("value");
                        UpdateProperty(car, field, value);
                    }
                }
                else
                    return BadRequest("Érvénytelen JSON formátum.");

                await context.SaveChangesAsync();
                return Ok($"A(z) {id} ID-val rendelkező autó frissítésre került!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba az autó frissítése során: {ex.Message}");
            }
        }

        private void UpdateProperty(Car car, string fieldName, JsonElement value)
        {
            try
            {
                var property = typeof(Car).GetProperty(fieldName,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (property == null)
                    throw new Exception($"Ismeretlen mező: {fieldName}");
                var convertedValue = JsonSerializer.Deserialize(value.GetRawText(), property.PropertyType);
                property.SetValue(car, convertedValue);
            }
            catch (Exception ex)
            {
                throw new Exception($"Hiba a(z) {fieldName} mező frissítése során: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var car = await context.Cars.FindAsync(id);
                if (car == null)
                    return NotFound($"Nem található autó a(z) {id} ID-val!");

                var equipment = await context.AdditionalEquipments.FirstOrDefaultAsync(x => x.CarId == id);
                if (equipment != null)
                    context.AdditionalEquipments.Remove(equipment);
                context.Cars.Remove(car);
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok($"A {id} ID-val rendelkező autó sikeresen törölve!");
            }
            catch
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "Hiba történt a törlés során.");
            }
        }
    }
}
