using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JoiningPrac1.Models;
using JoiningPrac1.DTOs;

namespace JoiningPrac1
{
    public class EmployeeJoiningInput
    {
        public decimal ? Eid { get; set; }
    }
    public class EmployeeJoiningOutput
    {
        public decimal ? Eid { get; set; }
        public string Ename { get; set; }
        public string Dname { get; set; }
        public string Dgname { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ModelContext _context;

        public EmployeesController(ModelContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }
        [HttpPost("JoiningData")]
        public async Task<ActionResult<ResponseDto>> JoinEmployee([FromBody] EmployeeJoiningInput input)
        {
            if (input.Eid == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                {
                    Message = "Please Fill the Eid Field",
                    Success = false,
                    Payload = null
                });
            }
            List<EmployeeJoiningOutput> employeeJoiningOutputs =
                await (from em in _context.Employees
                                        .Where(i => i.Eid == input.Eid)
                       from dept in _context.Departments
                                        .Where(i => em.Did == i.Did)
                       from des in _context.Designations
                                        .Where(i => em.Dgid == i.Dgid)
                       select new EmployeeJoiningOutput
                       {
                           Eid = em.Eid,
                           Ename = em.Ename,
                           Dname = dept.Dname,
                           Dgname = des.Dgname
                       }).OrderBy(i => i.Eid).ToListAsync();
            if (employeeJoiningOutputs.Count <= 0)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseDto
                {
                    Message = "Data is not found for this ID",
                    Success = false,
                    Payload = null
                });
            }
            return StatusCode(StatusCodes.Status200OK, new ResponseDto
            {
                Message = "Joining Done",
                Success = true,
                Payload = employeeJoiningOutputs
            });
        }
        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(decimal? id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(decimal? id, Employee employee)
        {
            if (id != employee.Eid)
            {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EmployeeExists(employee.Eid))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEmployee", new { id = employee.Eid }, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(decimal? id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(decimal? id)
        {
            return _context.Employees.Any(e => e.Eid == id);
        }
    }
}
