using course.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace course.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaxiCallsController : ControllerBase
    {
        private readonly TaxiDbContext _context;

        public TaxiCallsController(TaxiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaxiCall>>> GetTaxiCalls([FromQuery] string? status = null)
        {
            try
            {
                var query = _context.TaxiCalls.AsQueryable();

                if (!string.IsNullOrEmpty(status))
                {
                    query = query.Where(c => c.Status.ToLower() == status.ToLower());
                }

                var calls = await query
                    .OrderByDescending(c => c.CallTime)
                    .ToListAsync();

                return Ok(calls);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ошибка при получении вызовов: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaxiCall>> GetTaxiCall(int id)
        {
            try
            {
                var call = await _context.TaxiCalls.FindAsync(id);

                if (call == null)
                {
                    return NotFound(new { message = "Вызов не найден" });
                }

                return Ok(call);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ошибка при получении вызова: {ex.Message}" });
            }
        }

        [HttpGet("recent")]
        public async Task<ActionResult<IEnumerable<TaxiCall>>> GetRecentCalls([FromQuery] int hours = 24)
        {
            try
            {
                var since = DateTime.UtcNow.AddHours(-hours);

                var calls = await _context.TaxiCalls
                    .Where(c => c.CallTime >= since)
                    .OrderByDescending(c => c.CallTime)
                    .ToListAsync();

                return Ok(calls);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ошибка при получении недавних вызовов: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<TaxiCall>> CreateTaxiCall([FromBody] TaxiCall call)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                call.CallTime = DateTime.UtcNow;
                call.CreatedAt = DateTime.UtcNow;
                call.UpdatedAt = DateTime.UtcNow;

                if (string.IsNullOrEmpty(call.Status))
                {
                    call.Status = "pending";
                }

                _context.TaxiCalls.Add(call);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetTaxiCall), new { id = call.Id }, call);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ошибка при создании вызова: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TaxiCall>> UpdateTaxiCall(int id, [FromBody] TaxiCall call)
        {
            try
            {
                if (id != call.Id)
                {
                    return BadRequest(new { message = "ID в URL не совпадает с ID в теле запроса" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingCall = await _context.TaxiCalls.FindAsync(id);
                if (existingCall == null)
                {
                    return NotFound(new { message = "Вызов не найден" });
                }

                existingCall.ClientName = call.ClientName;
                existingCall.ClientPhone = call.ClientPhone;
                existingCall.PickupAddress = call.PickupAddress;
                existingCall.DestinationAddress = call.DestinationAddress;
                existingCall.Status = call.Status ?? "pending";
                existingCall.DriverName = call.DriverName;
                existingCall.CarModel = call.CarModel;
                existingCall.CarNumber = call.CarNumber;
                existingCall.Price = call.Price;
                existingCall.Duration = call.Duration;
                existingCall.Distance = call.Distance;
                existingCall.Rating = call.Rating;
                existingCall.Notes = call.Notes;
                existingCall.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(existingCall);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ошибка при обновлении вызова: {ex.Message}" });
            }
        }

        [HttpPatch("{id}/status")]
        public async Task<ActionResult<TaxiCall>> UpdateCallStatus(int id, [FromBody] CallStatusUpdateDto statusUpdate)
        {
            try
            {
                var call = await _context.TaxiCalls.FindAsync(id);
                if (call == null)
                {
                    return NotFound(new { message = "Вызов не найден" });
                }

                call.Status = statusUpdate.Status.ToString().ToLower();
                call.UpdatedAt = DateTime.UtcNow;

                if (statusUpdate.Status == CallStatus.Completed && statusUpdate.Price.HasValue)
                {
                    call.Price = statusUpdate.Price.Value;
                }

                await _context.SaveChangesAsync();

                return Ok(call);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ошибка при обновлении статуса вызова: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaxiCall(int id)
        {
            try
            {
                var call = await _context.TaxiCalls.FindAsync(id);
                if (call == null)
                {
                    return NotFound(new { message = "Вызов не найден" });
                }

                _context.TaxiCalls.Remove(call);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Вызов успешно удален" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ошибка при удалении вызова: {ex.Message}" });
            }
        }
    }

    public class CallStatusUpdateDto
    {
        public CallStatus Status { get; set; }
        public decimal? Price { get; set; }
    }
}