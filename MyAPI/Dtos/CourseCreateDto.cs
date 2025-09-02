using MyAPI.Attibutes;
using System.ComponentModel.DataAnnotations;

namespace MyAPI.Dtos
{
    [CourseTitleDifferentDescription]
    public class CourseCreateDto //: IValidatableObject
    {
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Description { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (Title == Description)
        //    {
        //        yield return new ValidationResult(
        //            "The provided description should be different from the title.",
        //            new[] { "CourseCreateDto" });
        //    }
        //}
    }
}
