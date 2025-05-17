namespace api.Dtos.Course
{
    public class UpdateCourseRequestDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? Schedule { get; set; }
        public string? Professor { get; set; }
    }
}