using System;
using System.Collections.Generic;

namespace autoberles_backend.Models;

public partial class TransmissionType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}
