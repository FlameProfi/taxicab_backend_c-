using course.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace course.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarsController : BaseController
    {
        private readonly TaxiDbContext _context;

        public CarsController(TaxiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars()
        {
            try
            {
                var cars = await _context.Cars
                    .OrderByDescending(c => c.CreatedAt)
                    .ToListAsync();

                return Ok(cars);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ошибка при получении автомобилей: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> GetCar(int id)
        {
            try
            {
                var car = await _context.Cars.FindAsync(id);

                if (car == null)
                {
                    return NotFound(new { message = "Автомобиль не найден" });
                }

                return Ok(car);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ошибка при получении автомобиля: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Car>> CreateCar([FromBody] Car car)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                car.CreatedAt = DateTime.UtcNow;
                car.UpdatedAt = DateTime.UtcNow;

                _context.Cars.Add(car);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCar), new { id = car.Id }, car);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ошибка при создании автомобиля: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Car>> UpdateCar(int id, [FromBody] Car car)
        {
            try
            {
                if (id != car.Id)
                {
                    return BadRequest(new { message = "ID в URL не совпадает с ID в теле запроса" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingCar = await _context.Cars.FindAsync(id);
                if (existingCar == null)
                {
                    return NotFound(new { message = "Автомобиль не найден" });
                }

                existingCar.Make = car.Make;
                existingCar.Model = car.Model;
                existingCar.Year = car.Year;
                existingCar.Price = car.Price;
                existingCar.Color = car.Color;
                existingCar.ImageUrl = car.ImageUrl;
                existingCar.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(existingCar);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ошибка при обновлении автомобиля: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            try
            {
                var car = await _context.Cars.FindAsync(id);
                if (car == null)
                {
                    return NotFound(new { message = "Автомобиль не найден" });
                }

                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Автомобиль успешно удален" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ошибка при удалении автомобиля: {ex.Message}" });
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Car>>> SearchCars([FromQuery] string? make, [FromQuery] string? model)
        {
            try
            {
                var query = _context.Cars.AsQueryable();

                if (!string.IsNullOrEmpty(make))
                {
                    query = query.Where(c => c.Make.ToLower().Contains(make.ToLower()));
                }

                if (!string.IsNullOrEmpty(model))
                {
                    query = query.Where(c => c.Model.ToLower().Contains(model.ToLower()));
                }

                var cars = await query
                    .OrderByDescending(c => c.CreatedAt)
                    .ToListAsync();

                return Ok(cars);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ошибка при поиске автомобилей: {ex.Message}" });
            }
        }
    }
}