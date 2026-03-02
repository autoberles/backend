using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace autoberles_backend.Models;

[Index(nameof(City), IsUnique = true)]
[Index(nameof(Address), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
[Index(nameof(PhoneNumber), IsUnique = true)]
public partial class Branch
{
    [Key]
    public int Id { get; set; }

    
    [Required(ErrorMessage = "A város megadása kötelező.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "A város neve 2 és 100 karakter között kell legyen.")]
    public string City { get; set; } = null!;


    [Required(ErrorMessage = "A cím megadása kötelező.")]
    [StringLength(255, MinimumLength = 5, ErrorMessage = "A cím legalább 5 karakter hosszú kell legyen.")]
    public string Address { get; set; } = null!;


    [Required(ErrorMessage = "Az e-mail cím megadása kötelező.")]
    [EmailAddress(ErrorMessage = "Érvénytelen e-mail formátum.")]
    [StringLength(150, ErrorMessage = "Az e-mail cím legfeljebb 150 karakter lehet.")]
    public string Email { get; set; } = null!;


    [Required(ErrorMessage = "A telefonszám megadása kötelező.")]
    [RegularExpression(@"^(\+36|06)[0-9]{1,2} ?[0-9]{3} ?[0-9]{3,4}$", ErrorMessage = "Érvénytelen telefonszám formátum.")]
    public string PhoneNumber { get; set; } = null!;

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}
