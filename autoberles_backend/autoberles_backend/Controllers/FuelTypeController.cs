using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace autoberles_backend.Controllers
{
    [Route("api/fuel_types")]
    [ApiController]
    public class FuelTypeController : ControllerBase
    {
        private readonly CarRentalContext _context;
        public FuelTypeController(CarRentalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFuelTypes()
        {
            var fuelTypes = await _context.FuelTypes.ToListAsync();
            if (fuelTypes == null)
                return BadRequest("Hiba az üzemanyagtípusok lekérdezése során");
            return Ok(fuelTypes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFuelTypeById([FromRoute] int id)
        {
            try
            {
                var fuelType = await _context.FuelTypes.FirstOrDefaultAsync(x => x.Id == id);
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
