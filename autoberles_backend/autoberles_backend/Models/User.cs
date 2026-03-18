using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace autoberles_backend.Models;


[Index(nameof(Email), IsUnique = true)]
public partial class User
{
    [Key]
    public int Id { get; set; }


    [Required(ErrorMessage = "A vezetéknév megadása kötelező.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "A vezetéknév hosszának 2 és 100 karakter közöttinek kell lennie.")]
    public string LastName { get; set; } = null!;


    [Required(ErrorMessage = "A keresztnév megadása kötelező.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "A keresztnév hosszának 2 és 100 karakter közöttinek kell lennie.")]
    public string FirstName { get; set; } = null!;


    [Required(ErrorMessage = "Az e-mail cím megadása kötelező.")]
    [EmailAddress(ErrorMessage = "Érvénytelen e-mail formátum.")]
    [StringLength(150, ErrorMessage = "Az e-mail cím legfeljebb 150 karakter hosszú lehet.")]
    public string Email { get; set; } = null!;


    [Required(ErrorMessage = "A születési dátum megadása kötelező.")]
    [DataType(DataType.Date)]
    [CustomValidation(typeof(User), nameof(ValidateBirthDate))]
    public DateTime? BirthDate { get; set; }

    [Required(ErrorMessage = "A felhasználó szerepkörét kötelező megadni.")]
    [RegularExpression("^(customer|admin|agent)$",
        ErrorMessage = "A felhasználó szerepköre csak customer, admin vagy agent lehet.")]
    public string Role { get; set; } = "customer";


    [JsonIgnore]
    public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();

    public static ValidationResult? ValidateBirthDate(DateTime birthDate, ValidationContext context)
    {
        var age = DateTime.Today.Year - birthDate.Year;

        if (birthDate > DateTime.Today.AddYears(-age))
            age--;

        if (age < 18)
        {
            return new ValidationResult("A felhasználónak legalább 18 évesnek kell lennie.");
        }

        if (birthDate < new DateTime(1900, 1, 1))
        {
            return new ValidationResult("A születési dátum túl régi.");
        }

        return ValidationResult.Success;
    }
}
