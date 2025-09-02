using System.ComponentModel.DataAnnotations;

namespace MyAPI.Dtos
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int AuthorId { get; set; }

    }
}
