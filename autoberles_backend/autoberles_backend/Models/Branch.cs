using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace autoberles_backend.Models;


[Index(nameof(Address), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
[Index(nameof(PhoneNumber), IsUnique = true)]
public partial class Branch
{
    [Key]
    [BindNever]
    [SwaggerIgnore]
    public int Id { get; set; }

    [Required(ErrorMessage = "A város megadása kötelező.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "A város neve 2 és 100 karakter között kell legyen.")]
    public string City { get; set; } = null!;


    [Required(ErrorMessage = "A cím megadása kötelező.")]
    [StringLength(255, MinimumLength = 5, ErrorMessage = "A címnek legalább 5 karakter hosszúnak kell lennie.")]
    public string Address { get; set; } = null!;


    [Required(ErrorMessage = "Az e-mail cím megadása kötelező.")]
    [EmailAddress(ErrorMessage = "Érvénytelen e-mail formátum.")]
    [StringLength(150, ErrorMessage = "Az e-mail cím legfeljebb 150 karakter lehet.")]
    public string Email { get; set; } = null!;


    [Required(ErrorMessage = "A telefonszám megadása kötelező.")]
    [RegularExpression(@"^\+36\s\d{2}\s\d{3}\s\d{4}$",
        ErrorMessage = "A telefonszám kötelező formátuma: +36 20 123 4567")]
    public string PhoneNumber { get; set; } = null!;


    [JsonIgnore]
    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}
