using System.ComponentModel.DataAnnotations;

namespace MyAPI.Dtos
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string? MainCategory { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }
    }
}
