using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.CustomValidators
{
    public class DueDateValidator
    {
        public static ValidationResult ValidateDueDate(DateTime dueDate,ValidationContext context) 
        {
            if (dueDate<DateTime.Now)
            {
                return new ValidationResult("Due Date cannot be earlier than the current date and time");
            }
            return ValidationResult.Success;
        }
    }
}
