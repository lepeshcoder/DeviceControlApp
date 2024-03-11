using System.ComponentModel.DataAnnotations;

namespace DeviceControlApp.DAL.ValidationAttributes;

public class DateCannotBeInPastAttribute : ValidationAttribute
{
    
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value != null)
        {
            DateTime dateValue = (DateTime)value;
            if (dateValue < DateTime.Now)
            {
                return new ValidationResult("Date cannot be in the past.");
            }
        }
        return ValidationResult.Success;
    }
}