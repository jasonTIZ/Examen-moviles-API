using api.Models;

public class Student
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }

    // Relación uno a muchos (un curso puede tener muchos estudiantes, pero cada estudiante solo uno)
    public int CourseId { get; set; }         // Clave foránea
    public Course Course { get; set; } = null!;
}