namespace api.Dtos.Student
{
    public class CreateStudentRequestDto
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }

        // Un solo curso asignado
        public required int CourseId { get; set; }
    }
}
