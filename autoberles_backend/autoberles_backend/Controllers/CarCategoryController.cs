using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace autoberles_backend.Controllers
{
    [Route("api/car_categories")]
    [ApiController]
    public class CarCategoryController : ControllerBase
    {
        CarRentalContext context = new CarRentalContext();

        [HttpGet]
        public async Task<IActionResult> GetAllCarCategories()
        {
            var carCategories = await context.CarCategories.ToListAsync();
            if (carCategories == null)
                return BadRequest("Hiba az autókategóriák lekérdezése során");
            return Ok(carCategories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarCategoryById([FromRoute] int id)
        {
            try
            {
                var carCategory = await context.CarCategories.FirstOrDefaultAsync(x => x.Id == id);
                if (carCategory == null)
                    return NotFound($"Nem található autókategória a(z) {id} ID-val!");
                return Ok(carCategory);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba {ex}");
            }
        }
    }
}
