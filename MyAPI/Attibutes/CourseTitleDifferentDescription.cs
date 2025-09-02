using System.ComponentModel.DataAnnotations;

namespace MyAPI.Attibutes
{
    public class CourseTitleDifferentDescription :ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var course = (Dtos.CourseCreateDto)validationContext.ObjectInstance;
            if (course.Title == course.Description)
            {
                return new ValidationResult("The provided description should be different from the title.", 
                new[] { "CourseCreateDto" });
            }
            return ValidationResult.Success;
        }
    }
}
