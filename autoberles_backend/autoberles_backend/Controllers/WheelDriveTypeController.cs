using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace autoberles_backend.Controllers
{
    [Route("api/wheel_drive_types")]
    [ApiController]
    public class WheelDriveTypeController : ControllerBase
    {
        CarRentalContext context = new CarRentalContext();

        [HttpGet]
        public async Task<IActionResult> GetAllWheelDriveTypes()
        {
            var wheelDriveTypes = await context.WheelDriveTypes.ToListAsync();
            if (wheelDriveTypes == null)
                return BadRequest("Hiba a kerékmeghajtások lekérdezése során");
            return Ok(wheelDriveTypes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWheelDriveTypeById([FromRoute] int id)
        {
            try
            {
                var wheelDriveType = await context.WheelDriveTypes.FirstOrDefaultAsync(x => x.Id == id);
                if (wheelDriveType == null)
                    return NotFound($"Nem található kerékmeghajtás a(z) {id} ID-val!");
                return Ok(wheelDriveType);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba {ex}");
            }
        }
    }
}
