using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace autoberles_backend.Models;


[Index(nameof(Name), IsUnique = true)]
public partial class AirConditioningType
{
    [Key]
    public int Id { get; set; }


    [Required(ErrorMessage = "A légkondícionáló típusának megadása kötelező.")]
    public string Name { get; set; } = null!;


    [JsonIgnore]
    public virtual ICollection<AdditionalEquipment> AdditionalEquipments { get; set; } = new List<AdditionalEquipment>();
}
