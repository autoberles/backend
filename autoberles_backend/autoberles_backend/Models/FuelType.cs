using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace autoberles_backend.Models;


[Index(nameof(Name), IsUnique = true)]
public partial class FuelType
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Az autó üzemanyagtípusát kötelező megadni.")]
    public string Name { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}
