using api.Dtos.Course;

namespace api.Dtos.Student
{
    public class StudentDto
    {
        public int? Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }

        // Lista de cursos asignados
        public required List<CourseDto> Courses { get; set; }
    }
}
