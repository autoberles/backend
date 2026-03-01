using System;
using System.Collections.Generic;

namespace autoberles_backend.Models;

public partial class User
{
    public int Id { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime BirthDate { get; set; }

    public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}
