using System;
using System.Collections.Generic;

namespace autoberles_backend.Models;

public partial class Branch
{
    public int Id { get; set; }

    public string City { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}
