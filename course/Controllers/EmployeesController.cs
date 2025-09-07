using course.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace course.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : BaseController
    {
        private readonly TaxiDbContext _context;

        public EmployeesController(TaxiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            try
            {
                var employees = await _context.Employees
                    .OrderBy(e => e.LastName)
                    .ToListAsync();

                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ошибка при получении сотрудников: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id);

                if (employee == null)
                {
                    return NotFound(new { message = "Сотрудник не найден" });
                }

                return Ok(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ошибка при получении сотрудника: {ex.Message}" });
            }
        }

        [HttpGet("department/{department}")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployeesByDepartment(string department)
        {
            try
            {
                var employees = await _context.Employees
                    .Where(e => e.Department.ToLower() == department.ToLower())
                    .OrderBy(e => e.LastName)
                    .ToListAsync();

                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ошибка при получении сотрудников по отделу: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee([FromBody] Employee employee)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!string.IsNullOrEmpty(employee.Email))
                {
                    var existingEmployee = await _context.Employees
                        .FirstOrDefaultAsync(e => e.Email.ToLower() == employee.Email.ToLower());

                    if (existingEmployee != null)
                    {
                        return BadRequest(new { message = "Сотрудник с таким email уже существует" });
                    }
                }

                employee.CreatedAt = DateTime.UtcNow;
                employee.UpdatedAt = DateTime.UtcNow;

                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ошибка при создании сотрудника: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Employee>> UpdateEmployee(int id, [FromBody] Employee employee)
        {
            try
            {
                if (id != employee.Id)
                {
                    return BadRequest(new { message = "ID в URL не совпадает с ID в теле запроса" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingEmployee = await _context.Employees.FindAsync(id);
                if (existingEmployee == null)
                {
                    return NotFound(new { message = "Сотрудник не найден" });
                }

                if (!string.IsNullOrEmpty(employee.Email) && employee.Email != existingEmployee.Email)
                {
                    var emailExists = await _context.Employees
                        .AnyAsync(e => e.Email.ToLower() == employee.Email.ToLower() && e.Id != id);

                    if (emailExists)
                    {
                        return BadRequest(new { message = "Сотрудник с таким email уже существует" });
                    }
                }

                existingEmployee.FirstName = employee.FirstName;
                existingEmployee.LastName = employee.LastName;
                existingEmployee.Position = employee.Position;
                existingEmployee.Department = employee.Department;
                existingEmployee.Email = employee.Email;
                existingEmployee.Phone = employee.Phone;
                existingEmployee.Salary = employee.Salary;
                existingEmployee.HireDate = employee.HireDate;
                existingEmployee.ImageUrl = employee.ImageUrl;
                existingEmployee.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(existingEmployee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ошибка при обновлении сотрудника: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id);
                if (employee == null)
                {
                    return NotFound(new { message = "Сотрудник не найден" });
                }

                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Сотрудник успешно удален" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ошибка при удалении сотрудника: {ex.Message}" });
            }
        }
    }
}