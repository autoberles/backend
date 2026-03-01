using System;
using System.Collections.Generic;

namespace autoberles_backend.Models;

public partial class Rental
{
    public int Id { get; set; }

    public int CarId { get; set; }

    public int UserId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public virtual Car Car { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
