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
    public class CourseController : ControllerBase
    {
        private readonly DataBaseContext _context;
        private readonly ILogger<StudentController> _logger;

        public CourseController(DataBaseContext context, ILogger<StudentController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourse(int id)
        {
            try
            {
                var course = await _context.Courses.FindAsync(id);
                if (course == null)
                {
                    _logger.LogWarning("Course with ID {Id} not found.", id);
                    return NotFound($"Course with ID {id} not found.");
                }

                _logger.LogInformation("Successfully retrieved course with ID {Id}.", id);
                return Ok(course);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the course.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CourseDto courseDto)
        {
            if (courseDto == null)
            {
                _logger.LogWarning("Received null course object.");
                return BadRequest("Course object is null.");
            }

            if (string.IsNullOrWhiteSpace(courseDto.Title) || courseDto.Credits < 0)
            {
                _logger.LogWarning("Received invalid course object with empty Title.");
                return BadRequest("Course Title cannot be empty.");
            }
            var existingCourse = await _context.Courses.AnyAsync(c => c.Title == courseDto.Title);
            if (existingCourse)
            {
                _logger.LogWarning("Course with title {Title} already exists.", courseDto.Title);
                return Conflict($"Course with title '{courseDto.Title}' already exists.");
            }
            try
            {
                var course = new Course
                {
                    Title = courseDto.Title,
                    Credits = courseDto.Credits
                };

                _context.Courses.Add(course);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully created course with ID {Id}.", course.CourseId);
                return CreatedAtAction(nameof(GetCourse), new { id = course.CourseId }, course);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the course.");
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] CourseDto courseDto)
        {
            if (courseDto == null)
            {
                _logger.LogWarning("Received null course object for update.");
                return BadRequest("Course object is null.");
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                _logger.LogWarning("Course with ID {Id} not found for update.", id);
                return NotFound($"Course with ID {id} not found.");
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(courseDto.Title))
                {

                    course.Title = courseDto.Title;
                }
                if (courseDto.Credits >= 0)
                {
                    course.Credits = courseDto.Credits;
                }

                _context.Entry(course).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully updated course with ID {Id}.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the course.");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetCourses()
        {
            try
            {
                var courses = await _context.Courses.ToListAsync();
                _logger.LogInformation("Successfully retrieved all courses.");
                return Ok(courses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the courses.");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            try
            {
                // var course = await _context.Courses.FindAsync(id);
                // if (course == null)
                // {
                //     _logger.LogWarning("Course with ID {Id} not found for deletion.", id);
                //     return NotFound($"Course with ID {id} not found.");
                // }

                // _context.Courses.Remove(course);
                // to optimize the deletion process, we can use Entry to set the state to Deleted
                // and reduce sql queries
                Course courseToDelete = new Course() { CourseId = id };
                _context.Entry(courseToDelete).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully deleted course with ID {Id}.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the course.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}