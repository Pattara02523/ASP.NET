using System.ComponentModel.DataAnnotations;

namespace MyAPI.Dtos
{
    public class AuthorCreateDto
    {
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public DateTimeOffset DateOfBirth { get; set; }
        [Required]
        public string? MainCategory { get; set; }
    }
}
