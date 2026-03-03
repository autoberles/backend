using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace autoberles_backend.Models;

public partial class AdditionalEquipment
{
    [Key]
    public int Id { get; set; }

    public int CarId { get; set; }


    [Required]
    public bool ParkingSensors { get; set; }


    [Required(ErrorMessage = "A légkondicionálás típus megadása kötelező.")]
    [Range(1, int.MaxValue, ErrorMessage = "A légkondicionálás azonosítója érvényes kell legyen.")]
    public int AirConditioningId { get; set; }


    [Required]
    public bool HeatedSeats { get; set; }


    [Required]
    public bool Navigation { get; set; }


    [Required]
    public bool LeatherSeats { get; set; }

    public virtual AirConditioningType AirConditioning { get; set; } = null!;

    [JsonIgnore]
    public virtual Car Car { get; set; } = null!;
}
