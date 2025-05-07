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
                .Include(s => s.Course)
                .Where(s => s.CourseId == courseId)
                .Select(s => s.ToDto())
                .ToListAsync();

            return Ok(students);
        }

        // GET: api/Student/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> GetStudent(int id)
        {
            var student = await _context.Students
                .Include(s => s.Course)
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
            var course = await _context.Courses.FindAsync(dto.CourseId);

            if (course == null)
            {
                return BadRequest(new
                {
                    status = "error",
                    message = "ID de curso no válido."
                });
            }

            var student = dto.ToStudentFromCreateDto(course);

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            var studentDto = student.ToDto();

            try
            {
                await FirebaseHelper.SendPushNotificationToTopicAsync(
                    topic: "primer_examen",
                    title: "Nueva inscripción!",
                    body: $"Estudiante \"{student.Name}\" se ha registrado en el curso \"{course.Name}\"."
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
                .Include(s => s.Course)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return NotFound(new { status = "error", message = $"Estudiante con ID {id} no encontrado." });
            }

            var course = await _context.Courses.FindAsync(dto.CourseId);

            if (course == null)
            {
                return BadRequest(new { status = "error", message = "ID de curso no válido." });
            }

            student.UpdateFromDto(dto, course);
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
