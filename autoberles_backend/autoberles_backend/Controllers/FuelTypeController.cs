using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace autoberles_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuelTypeController : ControllerBase
    {
        CarRentalContext context = new CarRentalContext();

        [HttpGet("fuel_type")]
        public async Task<IActionResult> GetAllFuelTypes()
        {
            var fuelTypes = await context.FuelTypes.ToListAsync();
            if (fuelTypes == null)
                return BadRequest("Hiba az üzemanyagtípusok lekérdezése során");
            return Ok(fuelTypes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFuelTypeById([FromRoute] int id)
        {
            try
            {
                var fuelType = await context.FuelTypes.FirstOrDefaultAsync(x => x.Id == id);
                if (fuelType == null)
                    return NotFound($"Nem található üzemanyagtípus a(z) {id} ID-val!");
                return Ok(fuelType);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba {ex}");
            }
        }
    }
}
