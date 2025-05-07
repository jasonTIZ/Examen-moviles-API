namespace api.Dtos.Student
{
    public class UpdateStudentRequestDto
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }

        // Lista de cursos actualizados
        public required List<int> CourseIds { get; set; }
    }
}
