using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace autoberles_backend.Models;

public partial class AirConditioningType
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public virtual ICollection<AdditionalEquipment> AdditionalEquipments { get; set; } = new List<AdditionalEquipment>();
}
