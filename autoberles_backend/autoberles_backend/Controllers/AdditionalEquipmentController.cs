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
    public class AdditionalEquipmentController : ControllerBase
    {
        CarRentalContext context = new CarRentalContext();

        [HttpGet("additional_equipment")]
        public async Task<IActionResult> GetAllAdditionalEquipments()
        {
            var aes = await context.AdditionalEquipments.Include(x => x.AirConditioning).ToListAsync();
            if (aes == null)
                return BadRequest("Hiba az extra felszereltségek lekérdezése során");
            return Ok(aes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAdditionalEquipmentsById([FromRoute] int id)
        {
            try
            {
                var ae = await context.AdditionalEquipments
                            .Include(x => x.AirConditioning)
                            .FirstOrDefaultAsync(x => x.Id == id);
                if (ae == null)
                    return NotFound($"Nem található extra felszereltség a(z) {id} ID-val!");
                return Ok(ae);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba {ex}");
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchAdditionalEquipment(int id, [FromBody] JsonElement body)
        {
            try
            {
                var equipment = await context.AdditionalEquipments.FindAsync(id);
                if (equipment == null)
                    return NotFound($"Nem található felszereltség a(z) {id} ID-val!");
                if (body.ValueKind != JsonValueKind.Object)
                    return BadRequest("Érvénytelen JSON formátum!");

                foreach (var property in body.EnumerateObject())
                {
                    string propertyName = ConvertSnakeToPascal(property.Name);

                    var propInfo = typeof(AdditionalEquipment).GetProperty(
                        propertyName,
                        BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (propInfo == null)
                        return BadRequest($"Ismeretlen mező: {property.Name}");

                    var convertedValue = JsonSerializer.Deserialize(
                        property.Value.GetRawText(),
                        propInfo.PropertyType);

                    propInfo.SetValue(equipment, convertedValue);
                }

                await context.SaveChangesAsync();
                return Ok($"A(z) {id} ID-val rendelkező felszereltség frissítésre került.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba a felszereltség frissítése során: {ex.Message}");
            }
        }

        private string ConvertSnakeToPascal(string snakeCase)
        {
            return string.Join("", snakeCase.Split('_').Select(word => char.ToUpper(word[0]) + word.Substring(1)));
        }
    }
}
