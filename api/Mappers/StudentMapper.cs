using api.Dtos.Student;
using api.Models;

namespace api.Mappers
{
    public static class StudentMapper
    {
        public static StudentDto ToDto(this Student student)
        {
            return new StudentDto
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                Phone = student.Phone,
                Courses = student.Courses.Select(c => c.ToDto()).ToList()
            };
        }

        public static Student ToStudentFromCreateDto(this CreateStudentRequestDto dto, List<Course> courses)
        {
            return new Student
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                Courses = courses
            };
        }

        public static void UpdateFromDto(this Student student, UpdateStudentRequestDto dto, List<Course> courses)
        {
            student.Name = dto.Name;
            student.Email = dto.Email;
            student.Phone = dto.Phone;
            student.Courses = courses;
        }

    }
}
