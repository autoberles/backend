using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace autoberles_backend.Controllers
{
    [Route("api/air_conditioning_types")]
    [ApiController]
    public class AirConditioningTypeController : ControllerBase
    {
        private readonly CarRentalContext _context;
        public AirConditioningTypeController(CarRentalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAirConditioningTypes()
        {
            var acts = await _context.AirConditioningTypes.ToListAsync();
            if (acts == null)
                return BadRequest("Hiba a klímatípusok lekérdezése során");
            return Ok(acts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAirConditionalTypeById([FromRoute] int id)
        {
            try
            {
                var act = await _context.AirConditioningTypes.FirstOrDefaultAsync(x => x.Id == id);
                if (act == null)
                    return NotFound($"Nem található klímatípus a(z) {id} ID-val!");
                return Ok(act);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba {ex}");
            }
        }
    }
}
