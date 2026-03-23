using autoberles_backend.Classes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace autoberles_backend.Models;


[Index(nameof(Email), IsUnique = true)]
[Index(nameof(PhoneNumber), IsUnique = true)]
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

    public string PasswordHash { get; set; } = null!;

    public string? ResetToken { get; set; }

    public DateTime? ResetTokenExpiry { get; set; }


    [Required(ErrorMessage = "A telefonszám megadása kötelező.")]
    [RegularExpression(@"^\+36\s\d{2}\s\d{3}\s\d{4}$",
        ErrorMessage = "Formátum: +36 20 123 4567")]
    public string PhoneNumber { get; set; } = null!;


    [Required(ErrorMessage = "A születési dátum megadása kötelező.")]
    [DataType(DataType.Date)]
    [CustomValidation(typeof(UserValidator), nameof(UserValidator.ValidateBirthDate))]
    public DateTime BirthDate { get; set; }


    [Required(ErrorMessage = "A felhasználó szerepkörét kötelező megadni.")]
    [RegularExpression("^(agent|admin|customer)$",
        ErrorMessage = "A felhasználó szerepköre csak customer, admin vagy agent lehet.")]
    public string Role { get; set; } = "customer";


    [JsonIgnore]
    public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}
