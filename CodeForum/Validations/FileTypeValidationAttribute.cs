using System.ComponentModel.DataAnnotations;

namespace CodeForum.Validations;

public class FileTypeValidationAttribute : ValidationAttribute
{
    private readonly string[] _validTypes;

    public FileTypeValidationAttribute(params string[] validTypes)
    {
        _validTypes = validTypes;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.Name).ToLowerInvariant();
            if (!_validTypes.Any(validType => fileExtension.EndsWith(validType)))
            {
                return new ValidationResult($"File type must be one of: {string.Join(", ", _validTypes)}");
            }
        }
        return ValidationResult.Success;
    }
}
