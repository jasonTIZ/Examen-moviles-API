using api.Models;

public class Student
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }

    // Relación muchos a muchos
    public ICollection<Course> Courses { get; set; } = [];
}
