using course.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace course.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriverApplicationsController : BaseController
    {
        private readonly TaxiDbContext _context;

        public DriverApplicationsController(TaxiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DriverApplication>>> GetApplications([FromQuery] string? status = null)
        {
            try
            {
                var query = _context.DriverApplications.AsQueryable();

                if (!string.IsNullOrEmpty(status))
                {
                    query = query.Where(a => a.Status.ToLower() == status.ToLower());
                }

                var applications = await query
                    .OrderByDescending(a => a.CreatedAt)
                    .ToListAsync();

                return Ok(applications);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ошибка при получении заявок: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DriverApplication>> GetApplication(int id)
        {
            try
            {
                var application = await _context.DriverApplications.FindAsync(id);

                if (application == null)
                {
                    return NotFound(new { message = "Заявка не найдена" });
                }

                return Ok(application);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ошибка при получении заявки: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<DriverApplication>> CreateApplication([FromBody] DriverApplication application)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                application.CreatedAt = DateTime.UtcNow;
                application.UpdatedAt = DateTime.UtcNow;
                application.Status = "pending"; 

                _context.DriverApplications.Add(application);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetApplication), new { id = application.Id }, application);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ошибка при создании заявки: {ex.Message}" });
            }
        }

        [HttpPut("{id}/status")]
        public async Task<ActionResult<DriverApplication>> UpdateApplicationStatus(int id, [FromBody] ApplicationStatusUpdateDto statusUpdate)
        {
            try
            {
                var application = await _context.DriverApplications.FindAsync(id);
                if (application == null)
                {
                    return NotFound(new { message = "Заявка не найдена" });
                }

                application.Status = statusUpdate.Status.ToLower();
                application.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Статус заявки успешно обновлен",
                    application = application
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ошибка при обновлении статуса заявки: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplication(int id)
        {
            try
            {
                var application = await _context.DriverApplications.FindAsync(id);
                if (application == null)
                {
                    return NotFound(new { message = "Заявка не найдена" });
                }

                _context.DriverApplications.Remove(application);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Заявка успешно удалена" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ошибка при удалении заявки: {ex.Message}" });
            }
        }

        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetApplicationStats()
        {
            try
            {
                var stats = new
                {
                    Total = await _context.DriverApplications.CountAsync(),
                    Pending = await _context.DriverApplications.CountAsync(a => a.Status == "pending"),
                    Approved = await _context.DriverApplications.CountAsync(a => a.Status == "approved"),
                    Rejected = await _context.DriverApplications.CountAsync(a => a.Status == "rejected")
                };

                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ошибка при получении статистики заявок: {ex.Message}" });
            }
        }
    }

    public class ApplicationStatusUpdateDto
    {
        public string Status { get; set; } = string.Empty;
    }
}