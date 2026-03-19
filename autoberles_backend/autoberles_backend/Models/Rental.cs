using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace autoberles_backend.Models;

public partial class Rental
{
    [Key]
    [BindNever]
    [SwaggerIgnore]
    public int Id { get; set; }

    public int? CarId { get; set; }

    [BindNever]
    [SwaggerIgnore]
    public int? UserId { get; set; }


    [Required(ErrorMessage = "A bérlés kezdetét megadni kötelező!")]
    [DataType(DataType.Date)]
    public DateTime? StartDate { get; set; }


    [Required(ErrorMessage = "A bérlés végét megadni kötelező!")]
    [DataType(DataType.Date)]
    public DateTime? EndDate { get; set; }

    [BindNever]
    [SwaggerIgnore]
    [DataType(DataType.Date)]
    public DateTime? ReturnDate { get; set; }


    [BindNever]
    [SwaggerIgnore]
    public string? Damage { get; set; } = null;


    [BindNever]
    [SwaggerIgnore]
    public int FullPrice { get; set; }


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
