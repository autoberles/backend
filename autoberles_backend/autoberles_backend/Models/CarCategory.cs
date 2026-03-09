using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace autoberles_backend.Models;

public partial class CarCategory
{
    [Key]
    public int Id { get; set; }


    [Required(ErrorMessage = "Az autó kategóriájának megadása kötelező.")]
    public string Name { get; set; } = null!;


    [JsonIgnore]
    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}
