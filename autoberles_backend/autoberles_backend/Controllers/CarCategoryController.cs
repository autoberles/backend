using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace autoberles_backend.Controllers
{
    [Route("api/car_categories")]
    [ApiController]
    public class CarCategoryController : ControllerBase
    {
        private readonly CarRentalContext _context;
        public CarCategoryController(CarRentalContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCarCategories()
        {
            var carCategories = await _context.CarCategories.ToListAsync();
            if (carCategories == null)
                return BadRequest("Hiba az autókategóriák lekérdezése során");
            return Ok(carCategories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarCategoryById([FromRoute] int id)
        {
            try
            {
                var carCategory = await _context.CarCategories.FirstOrDefaultAsync(x => x.Id == id);
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
