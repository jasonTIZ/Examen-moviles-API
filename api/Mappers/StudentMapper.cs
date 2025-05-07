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
                Course = student.Course.ToDto() // Cambiado: una sola instancia
            };
        }

        public static Student ToStudentFromCreateDto(this CreateStudentRequestDto dto, Course course)
        {
            return new Student
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                CourseId = course.Id,
                Course = course
            };
        }

        public static void UpdateFromDto(this Student student, UpdateStudentRequestDto dto, Course course)
        {
            student.Name = dto.Name;
            student.Email = dto.Email;
            student.Phone = dto.Phone;
            student.CourseId = course.Id;
            student.Course = course;
        }
    }
}
