using System;
using System.Collections.Generic;

namespace autoberles_backend.Models;

public partial class AirConditioningType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<AdditionalEquipment> AdditionalEquipments { get; set; } = new List<AdditionalEquipment>();
}
