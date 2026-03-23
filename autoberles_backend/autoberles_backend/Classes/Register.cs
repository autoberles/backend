using autoberles_backend.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace autoberles_backend.Classes
{
    public class Register
    {

        [Required(ErrorMessage = "A keresztnév megadása kötelező.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "A keresztnév hosszának 2 és 100 karakter közöttinek kell lennie.")]
        public string FirstName { get; set; } = null!;


        [Required(ErrorMessage = "A vezetéknév megadása kötelező.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "A vezetéknév hosszának 2 és 100 karakter közöttinek kell lennie.")]
        public string LastName { get; set; } = null!;


        [Required(ErrorMessage = "Az e-mail cím megadása kötelező.")]
        [EmailAddress(ErrorMessage = "Érvénytelen e-mail formátum.")]
        [StringLength(150, ErrorMessage = "Az e-mail cím legfeljebb 150 karakter hosszú lehet.")]
        public string Email { get; set; } = null!;


        [Required(ErrorMessage = "A jelszó megadása kötelező.")]
        [MinLength(8, ErrorMessage = "A jelszónak legalább 8 karakter hosszúnak kell lennie.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
            ErrorMessage = "A jelszónak tartalmaznia kell kisbetűt, nagybetűt és számot.")]
        public string Password { get; set; } = null!;


        [Required(ErrorMessage = "A jelszót kötelező újra megadni.")]
        [Compare("Password", ErrorMessage = "A jelszavak nem egyeznek.")]
        public string ConfirmPassword { get; set; } = null!;


        [Required(ErrorMessage = "A telefonszám megadása kötelező.")]
        [RegularExpression(@"^\+36\s\d{2}\s\d{3}\s\d{4}$",
            ErrorMessage = "A telefonszám kötelező formátuma: +36 20 123 4567")]
        public string PhoneNumber { get; set; } = null!;


        [Required(ErrorMessage = "A születési dátum megadása kötelező.")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(UserValidator), nameof(UserValidator.ValidateBirthDate))]
        public DateTime BirthDate { get; set; }


        [Range(typeof(bool), "true", "true", ErrorMessage = "El kell fogadnia a felhasználási feltételeket.")]
        public bool AcceptTerms { get; set; }

    }
}
