using System.ComponentModel.DataAnnotations;

namespace MyAPI.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string?  FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string? LastName { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }

        [Required]
        [StringLength(100)]
        public string? MainCategory { get; set; }
    }
}
