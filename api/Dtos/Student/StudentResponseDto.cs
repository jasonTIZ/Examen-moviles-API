namespace api.Dtos.Student
{
    public class StudentResponseDto
    {
        public required string Status { get; set; }
        public required string Message { get; set; }
        public required StudentDto Student { get; set; }
    }
}