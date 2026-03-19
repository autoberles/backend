using System.ComponentModel.DataAnnotations;

namespace autoberles_backend.Classes
{
    public static class UserValidator
    {
        public static ValidationResult? ValidateBirthDate(DateTime birthDate, ValidationContext context)
        {
            var age = DateTime.Today.Year - birthDate.Year;

            if (birthDate > DateTime.Today.AddYears(-age))
                age--;

            if (age < 18)
                return new ValidationResult("A felhasználónak legalább 18 évesnek kell lennie.");

            if (birthDate < new DateTime(1900, 1, 1))
                return new ValidationResult("A születési dátum túl régi.");

            return ValidationResult.Success;
        }
    }
}
