using course.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace course.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly TaxiDbContext _context;

        public HealthController(TaxiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<object>> Get()
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();
                var carsCount = await _context.Cars.CountAsync();

                return Ok(new
                {
                    Status = "Healthy",
                    Database = canConnect ? "Connected" : "Disconnected",
                    CarsCount = carsCount,
                    Time = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = "Unhealthy",
                    Error = ex.Message,
                    Time = DateTime.UtcNow
                });
            }
        }
    }
}