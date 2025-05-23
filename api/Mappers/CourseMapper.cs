using api.Dtos.Course;
using api.Models;

namespace api.Mappers
{
    public static class CourseMapper
    {
        public static CourseDto ToDto(this Course course)
        {
            return new CourseDto
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                ImageUrl = course.ImageUrl,
                Schedule = course.Schedule,
                Professor = course.Professor
            };
        }

        public static Course ToCourseFromCreateDto(this CreateCourseRequestDto dto)
        {
            return new Course
            {
                Name = dto.Name,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                Schedule = dto.Schedule,
                Professor = dto.Professor
            };
        }

        public static void UpdateFromDto(this Course course, UpdateCourseRequestDto dto)
        {
            course.Name = dto.Name;
            course.Description = dto.Description;
            course.ImageUrl = dto.ImageUrl;
            course.Schedule = dto.Schedule;
            course.Professor = dto.Professor;
        }
    }
}