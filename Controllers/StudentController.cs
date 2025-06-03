using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Dtos;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {

        private readonly DataBaseContext _context;
        private readonly ILogger<StudentController> _logger;

        public StudentController(DataBaseContext context, ILogger<StudentController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent(int id)
        {
            try
            {
                var students = await _context.Students.Include(s => s.Enrollments).ThenInclude(e => e.Course).AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
                if (students == null)
                {
                    _logger.LogWarning("Student with ID {Id} not found.", id);
                    return NotFound($"Student with ID {id} not found.");
                }

                _logger.LogInformation("Successfully retrieved student with ID {Id}.", id);
                return Ok(students);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching students.");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] StudentDto studentDto)
        {
            if (studentDto == null)
            {
                _logger.LogWarning("Received null student object.");
                return BadRequest("Student object is null.");
            }
            if (string.IsNullOrWhiteSpace(studentDto.FirstName) || string.IsNullOrWhiteSpace(studentDto.LastName))
            {
                _logger.LogWarning("Received invalid student object with empty FirstName or LastName.");
                return BadRequest("Invalid student object.");
            }

            var existingStudent = await _context.Students.AnyAsync(s => s.FirstName == studentDto.FirstName && s.LastName == studentDto.LastName);
            if (existingStudent)
            {
                _logger.LogWarning("Student with name {FirstName} {LastName} already exists.", studentDto.FirstName, studentDto.LastName);
                return Conflict("Student already exists.");
            }

            _logger.LogInformation("Creating a new student with name {FirstName} {LastName}.", studentDto.FirstName, studentDto.LastName);

            try
            {
                var student = new Student
                {
                    FirstName = studentDto.FirstName,
                    LastName = studentDto.LastName,
                    EnrollmentDate = DateTime.UtcNow
                };
                {
                    _context.Students.Add(student);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Successfully created student with ID {Id}.", student.Id);
                    return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a student.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] StudentDto studentDto)
        {
            if (studentDto == null)
            {
                _logger.LogWarning("Received invalid student object for update.");
                return BadRequest("Invalid student object.");
            }

            try
            {
                var existingStudent = await _context.Students.FindAsync(id);
                if (existingStudent == null)
                {
                    _logger.LogWarning("Student with ID {Id} not found for update.", id);
                    return NotFound($"Student with ID {id} not found.");
                }

                if (!string.IsNullOrWhiteSpace(studentDto.FirstName))
                {
                    existingStudent.FirstName = studentDto.FirstName;
                }
                if (!string.IsNullOrWhiteSpace(studentDto.LastName))
                {
                    existingStudent.LastName = studentDto.LastName;
                }

                _context.Students.Update(existingStudent);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully updated student with ID {Id}.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a student.");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetStudents(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string sortBy = "FIrstName",
            [FromQuery] bool descending = false,
            [FromQuery] string? filter = null,
            [FromQuery] string? groupBy = null)

        {
            try
            {
                var query = _context.Students.Include(s => s.Enrollments).ThenInclude(e => e.Course).AsNoTracking();
                if (!string.IsNullOrWhiteSpace(filter))
                {
                    query = query.Where(s => s.FirstName.Contains(filter) || s.LastName.Contains(filter));
                }
                query = sortBy.ToLower() switch
                {
                    "lastname" => descending ? query.OrderByDescending(s => s.LastName) : query.OrderBy(s => s.LastName),
                    "enrollmentdate" => descending ? query.OrderByDescending(s => s.EnrollmentDate) : query.OrderBy(s => s.EnrollmentDate),
                    _ => descending ? query.OrderByDescending(s => s.FirstName) : query.OrderBy(s => s.FirstName)
                };

                var students = await query.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

                if (students == null || !students.Any())
                {
                    _logger.LogInformation("No students found.");
                    return NotFound("No students found.");
                }

                // grouping
                object result;
                if (!string.IsNullOrWhiteSpace(groupBy))
                {
                    switch (groupBy.ToLower())
                    {
                        case "enrollmentdate":
                            result = students.GroupBy(s => s.EnrollmentDate)
                         .Select(g => new
                         {
                             Key = g.Key,
                             Students = g.ToList()
                         });
                            break;

                        case "lastname":
                            result = students.GroupBy(s => s.LastName)
                                .Select(g => new
                                {
                                    Key = g.Key,
                                    Students = g.ToList()
                                });
                            break;
                        default:
                            result = students;
                            break;
                    }
                }

                _logger.LogInformation("Successfully retrieved {Count} students.", students.Count);
                return Ok(students);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching students.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {
                var student = await _context.Students.FindAsync(id);
                if (student == null)
                {
                    _logger.LogWarning("Student with ID {Id} not found for deletion.", id);
                    return NotFound($"Student with ID {id} not found.");
                }

                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully deleted student with ID {Id}.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a student.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

