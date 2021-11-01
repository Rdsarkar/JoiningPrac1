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
    public class Student2Controller : ControllerBase
    {
        private readonly ModelContext _context;

        public Student2Controller(ModelContext context)
        {
            _context = context;
        }

        // GET: api/Student2
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student2>>> GetStudent2s()
        {
            return await _context.Student2s.ToListAsync();
        }

        // GET: api/Student2/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student2>> GetStudent2(decimal? id)
        {
            var student2 = await _context.Student2s.FindAsync(id);

            if (student2 == null)
            {
                return NotFound();
            }

            return student2;
        }

        // PUT: api/Student2/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent2(decimal? id, Student2 student2)
        {
            if (id != student2.Id)
            {
                return BadRequest();
            }

            _context.Entry(student2).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Student2Exists(id))
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

        // POST: api/Student2
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Student2>> PostStudent2(Student2 student2)
        {
            _context.Student2s.Add(student2);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (Student2Exists(student2.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStudent2", new { id = student2.Id }, student2);
        }

        // DELETE: api/Student2/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent2(decimal? id)
        {
            var student2 = await _context.Student2s.FindAsync(id);
            if (student2 == null)
            {
                return NotFound();
            }

            _context.Student2s.Remove(student2);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Student2Exists(decimal? id)
        {
            return _context.Student2s.Any(e => e.Id == id);
        }
    }
}
