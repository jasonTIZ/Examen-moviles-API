using api.Data;
using api.Dtos.Student;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Student?courseId=5
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudentsByCourse([FromQuery] int courseId)
        {
            var students = await _context.Students
                .Include(s => s.Courses)
                .Where(s => s.Courses.Any(c => c.Id == courseId))
                .Select(s => s.ToDto())
                .ToListAsync();

            return Ok(students);
        }


        // GET: api/Student/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> GetStudent(int id)
        {
            var student = await _context.Students
                .Include(s => s.Courses)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return NotFound(new { status = "error", message = $"Estudiante con ID {id} no encontrado." });
            }

            return Ok(student.ToDto());
        }


        // POST: api/Student
        [HttpPost]
        public async Task<ActionResult<StudentResponseDto>> CreateStudent([FromBody] CreateStudentRequestDto dto)
        {
            // Validar que todos los cursos existen
            var courses = await _context.Courses
                .Where(c => dto.CourseIds.Contains(c.Id))
                .ToListAsync();

            if (courses.Count != dto.CourseIds.Count)
            {
                return BadRequest(new
                {
                    status = "error",
                    message = "Uno o más IDs de cursos no son válidos."
                });
            }

            var student = dto.ToStudentFromCreateDto(courses);

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            var studentDto = student.ToDto();

            // Notificación opcional
            try
            {
                await FirebaseHelper.SendPushNotificationToTopicAsync(
                    topic: "event_notifications",
                    title: "Nueva inscripción!",
                    body: $"Estudiante \"{student.Name}\" se ha registrado en {courses.Count} curso(s)."
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar notificación FCM: {ex.Message}");
            }

            return Ok(new StudentResponseDto
            {
                Status = "success",
                Message = $"Estudiante \"{student.Name}\" creado exitosamente.",
                Student = studentDto
            });
        }

        // PUT: api/Student/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<StudentResponseDto>> UpdateStudent(int id, [FromBody] UpdateStudentRequestDto dto)
        {
            var student = await _context.Students
                .Include(s => s.Courses)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return NotFound(new { status = "error", message = $"Estudiante con ID {id} no encontrado." });
            }

            var courses = await _context.Courses
                .Where(c => dto.CourseIds.Contains(c.Id))
                .ToListAsync();

            if (courses.Count != dto.CourseIds.Count)
            {
                return BadRequest(new { status = "error", message = "Uno o más IDs de cursos no son válidos." });
            }

            student.UpdateFromDto(dto, courses);
            await _context.SaveChangesAsync();

            return Ok(new StudentResponseDto
            {
                Status = "success",
                Message = $"Estudiante \"{student.Name}\" actualizado exitosamente.",
                Student = student.ToDto()
            });
        }

        // DELETE: api/Student/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return Ok(new { Message = $"Estudiante {student.Name} eliminado correctamente." });
        }
    }
}
