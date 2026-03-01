using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace autoberles_backend.Models;

public partial class FuelType
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}
