using api.Data;
using api.Dtos.Course;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses()
        {
            var courses = await _context.Courses
                .Include(c => c.Students)
                .Select(c => c.ToDto())
                .ToListAsync();

            return Ok(courses);
        }

        // GET: api/Courses/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourse(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            return Ok(course.ToDto());
        }

        // POST: api/Courses
        [HttpPost]
        public async Task<ActionResult<CourseDto>> CreateCourse([FromBody] CreateCourseRequestDto createCourseRequest)
        {
            var course = createCourseRequest.ToCourseFromCreateDto();

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, course.ToDto());
        }

        // PUT: api/Courses/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] CreateCourseRequestDto updateRequest)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            course.Name = updateRequest.Name;
            course.Description = updateRequest.Description;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Courses/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteCourseResponseDto>> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound(new DeleteCourseResponseDto
                {
                    Id = id,
                    Message = "Curso no encontrado."
                });
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return Ok(new DeleteCourseResponseDto
            {
                Id = id,
                Message = $"Curso con ID {id} eliminado exitosamente."
            });
        }
    }
}
