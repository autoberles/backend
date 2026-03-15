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


    [Required(ErrorMessage = "Kötelező megadni, hogy az autó rendelkezik-e parkolóérzékelővel.")]
    public bool ParkingSensors { get; set; }


    [Required(ErrorMessage = "A légkondicionálás típusának megadása kötelező.")]
    [Range(1, int.MaxValue, ErrorMessage = "A légkondicionálás azonosítója érvényes kell legyen.")]
    public int AirConditioningId { get; set; }


    [Required(ErrorMessage = "Kötelező megadni, hogy az autó rendelkezik-e fűtött üléssel.")]
    public bool HeatedSeats { get; set; }


    [Required(ErrorMessage = "Kötelező megadni, hogy az autó rendelkezik-e navigációval.")]
    public bool Navigation { get; set; }


    [Required(ErrorMessage = "Kötelező megadni, hogy az autó rendelkezik-e bőrüléssel.")]
    public bool LeatherSeats { get; set; }


    [Required(ErrorMessage = "Kötelező megadni, hogy az autó rendelkezik-e tempomattal.")]
    public bool Tempomat { get; set; }


    [JsonIgnore]
    public virtual AirConditioningType? AirConditioning { get; set; } = null!;

    [JsonIgnore]
    public virtual Car? Car { get; set; } = null!;
}
