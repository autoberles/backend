using autoberles_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection;
using System.Text.Json;

namespace autoberles_backend.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly CarRentalContext _context;
        public CarController(CarRentalContext context)
        {
            _context = context;
        }

        [HttpGet("cars")]
        public async Task<IActionResult> GetAllCars()
        {
            try
            {
                var now = DateTime.Now.Date;

                var cars = await _context.Cars
                    .Include(x => x.AdditionalEquipment)
                        .ThenInclude(x => x.AirConditioning)
                    .Include(x => x.Branch)
                    .Include(x => x.FuelType)
                    .Include(x => x.TransmissionType)
                    .Include(x => x.CarCategory)
                    .Include(x => x.WheelDriveType)
                    .Include(x => x.Rentals)
                    .ToListAsync();

                foreach (var car in cars)
                {
                    car.Availability = !car.Rentals.Any(r => r.ReturnDate == null && r.StartDate!.Value.Date <= now && now <= r.EndDate!.Value.Date);
                }
                return Ok(cars);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba az autók lekérdezése során: {ex.Message}");
            }
        }


        [HttpGet("cars/{id}")]
        public async Task<IActionResult> GetCarById([FromRoute] int id)
        {
            try
            {
                var car = await _context.Cars
                            .Include(x => x.AdditionalEquipment)
                                .ThenInclude(x => x.AirConditioning)
                            .Include(x => x.Branch)
                            .Include(x => x.FuelType)
                            .Include(x => x.TransmissionType)
                            .Include(x => x.CarCategory)
                            .Include(x => x.WheelDriveType)
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


        [Authorize(Roles = "admin")]
        [HttpPost("car")]
        public async Task<IActionResult> PostCar([FromBody] Car car)
        {
            IDbContextTransaction? transaction = null;
            if (_context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
                transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var equipment = car.AdditionalEquipment;
                car.AdditionalEquipment = null;
                car.Branch = null;
                car.FuelType = null;
                car.TransmissionType = null;
                car.CarCategory = null;
                car.WheelDriveType = null;

                _context.Cars.Add(car);
                await _context.SaveChangesAsync();

                if (equipment != null)
                {
                    equipment.CarId = car.Id;
                    _context.AdditionalEquipments.Add(equipment);
                    await _context.SaveChangesAsync();
                }

                if (transaction != null)
                    await transaction.CommitAsync();

                return CreatedAtAction(nameof(GetCarById), new { id = car.Id }, car);
            }
            catch (Exception ex)
            {
                if (transaction != null)
                    await transaction.RollbackAsync();

                return BadRequest($"Hiba az autó létrehozása során: {ex.Message}");
            }
        }


        [Authorize(Roles = "admin")]
        [HttpPost("car/image")]
        public async Task<IActionResult> UploadCarImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Nincs fájl feltöltve.");

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/cars");
            var originalFileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(folderPath, originalFileName);

            if (System.IO.File.Exists(filePath))
            {
                var existingUrl = $"{Request.Scheme}://{Request.Host}/cars/{originalFileName}";
                return Ok(new { url = existingUrl, reused = true });
            }

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var newFilePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(newFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var url = $"{Request.Scheme}://{Request.Host}/cars/{fileName}";
            return Ok(new { url, reused = false });
        }


        [Authorize(Roles = "admin")]
        [HttpGet("cars/images")]
        public IActionResult GetCarImages()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/cars");
            if (!Directory.Exists(path))
                return Ok(new List<string>());
            var files = Directory.GetFiles(path).Select(f => $"{Request.Scheme}://{Request.Host}/images/cars/{Path.GetFileName(f)}");
            return Ok(files);
        }


        [Authorize(Roles = "admin")]
        [HttpPatch("cars/{id}")]
        public async Task<IActionResult> PatchCar(int id, [FromBody] JsonElement body)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
                return NotFound($"Nem található autó a(z) {id} ID-val!");

            if (body.ValueKind != JsonValueKind.Object)
                return BadRequest("Érvénytelen JSON formátum!");

            foreach (var property in body.EnumerateObject())
            {
                var propName = ConvertSnakeToPascal(property.Name);

                var propInfo = typeof(Car).GetProperty(propName,
                    BindingFlags.Public | BindingFlags.Instance);
                if (propInfo == null)
                    return BadRequest($"Ismeretlen mező: {property.Name}");

                var value = JsonSerializer.Deserialize(property.Value.GetRawText(), propInfo.PropertyType);
                propInfo.SetValue(car, value);
            }

            await _context.SaveChangesAsync();
            return Ok($"A(z) {id} ID-val rendelkező autó frissítésre került!");
        }


        [Authorize(Roles = "admin")]
        [HttpDelete("cars/{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            IDbContextTransaction? transaction = null;
            if (_context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
                transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var car = await _context.Cars.FindAsync(id);
                if (car == null)
                    return NotFound($"Nem található autó a(z) {id} ID-val!");

                var equipment = await _context.AdditionalEquipments
                    .FirstOrDefaultAsync(x => x.CarId == id);
                if (equipment != null)
                    _context.AdditionalEquipments.Remove(equipment);

                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();

                if (transaction != null)
                    await transaction.CommitAsync();

                return Ok($"A {id} ID-val rendelkező autó sikeresen törölve!");
            }
            catch
            {
                if (transaction != null)
                    await transaction.RollbackAsync();

                return StatusCode(500, "Hiba történt a törlés során.");
            }
        }

        private string ConvertSnakeToPascal(string snakeCase)
        {
            return string.Join("", snakeCase.Split('_').Select(word => char.ToUpper(word[0]) + word.Substring(1)));
        }
    }
}
