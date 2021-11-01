using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JoiningPrac1.Models;

namespace JoiningPrac1
{
    [Route("api/[controller]")]
    [ApiController]
    public class Student1Controller : ControllerBase
    {
        private readonly ModelContext _context;

        public Student1Controller(ModelContext context)
        {
            _context = context;
        }

        // GET: api/Student1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student1>>> GetStudent1s()
        {
            return await _context.Student1s.ToListAsync();
        }

        // GET: api/Student1/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student1>> GetStudent1(decimal? id)
        {
            var student1 = await _context.Student1s.FindAsync(id);

            if (student1 == null)
            {
                return NotFound();
            }

            return student1;
        }

        // PUT: api/Student1/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent1(decimal? id, Student1 student1)
        {
            if (id != student1.Id)
            {
                return BadRequest();
            }

            _context.Entry(student1).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Student1Exists(id))
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

        // POST: api/Student1
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Student1>> PostStudent1(Student1 student1)
        {
            _context.Student1s.Add(student1);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (Student1Exists(student1.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStudent1", new { id = student1.Id }, student1);
        }

        // DELETE: api/Student1/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent1(decimal? id)
        {
            var student1 = await _context.Student1s.FindAsync(id);
            if (student1 == null)
            {
                return NotFound();
            }

            _context.Student1s.Remove(student1);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Student1Exists(decimal? id)
        {
            return _context.Student1s.Any(e => e.Id == id);
        }
    }
}
