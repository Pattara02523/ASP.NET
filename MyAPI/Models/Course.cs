using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAPI.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string? Title { get; set; }

        [Required]
        [StringLength(500)]
        public string? Description { get; set; }
        public int AuthorId { get; set; }

        //Navigation property
        [ForeignKey("AuthorId")]
        public Author? Author { get; set; }

    }
}
