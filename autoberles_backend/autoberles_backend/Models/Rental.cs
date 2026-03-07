using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace autoberles_backend.Models;

public partial class Rental
{
    [Key]
    public int Id { get; set; }

    public int? CarId { get; set; }

    public int? UserId { get; set; }


    [Required(ErrorMessage = "A bérlés kezdetét megadni kötelező!")]
    public DateTime? StartDate { get; set; }


    [Required(ErrorMessage = "A bérlés végét megadni kötelező!")]
    public DateTime? EndDate { get; set; }

    [JsonIgnore]
    public virtual Car? Car { get; set; } = null!;

    [JsonIgnore]
    public virtual User? User { get; set; } = null!;


    public static ValidationResult? ValidateDates(DateTime endDate, ValidationContext context)
    {
        var instance = (Rental)context.ObjectInstance;
        var startDate = instance.StartDate;

        if (startDate >= endDate)
        {
            return new ValidationResult("A kezdő dátumnak korábbinak kell lennie, mint a befejező dátumnak.");
        }

        if (startDate?.Date < DateTime.Today)
        {
            return new ValidationResult("A bérlés kezdete nem lehet múltbeli dátum.");
        }

        var days = (endDate - startDate)?.TotalDays;

        if (days < 1)
        {
            return new ValidationResult("A bérlés legalább 1 napos kell legyen.");
        }

        return ValidationResult.Success;
    }
}
