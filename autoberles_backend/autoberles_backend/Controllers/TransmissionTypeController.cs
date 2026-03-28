using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace autoberles_backend.Controllers
{
    [Route("api/transmission_types")]
    [ApiController]
    public class TransmissionTypeController : ControllerBase
    {
        private readonly CarRentalContext _context;
        public TransmissionTypeController(CarRentalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransmissionTypes()
        {
            var transmissionTypes = await _context.TransmissionTypes.ToListAsync();
            if (transmissionTypes == null)
                return BadRequest("Hiba a váltótípusok lekérdezése során");
            return Ok(transmissionTypes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransmissionTypeById([FromRoute] int id)
        {
            try
            {
                var transmissionType = await _context.TransmissionTypes.FirstOrDefaultAsync(x => x.Id == id);
                if (transmissionType == null)
                    return NotFound($"Nem található váltótípus a(z) {id} ID-val!");
                return Ok(transmissionType);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba {ex}");
            }
        }
    }
}
