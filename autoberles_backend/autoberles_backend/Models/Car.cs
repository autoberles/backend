using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace autoberles_backend.Models;

public class Car
{
    [Key]
    public int Id { get; set; }


    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Brand { get; set; } = null!;


    [Required]
    [StringLength (50, MinimumLength = 1)]
    public string Model { get; set; } = null!;


    [Required]
    [Range(1980,2100)]
    public int? Year { get; set; }


    [Required]
    [Range(2,9)]
    public short? NumberOfSeats { get; set; }


    [Required]
    public int? Price { get; set; }


    [Required]
    [RegularExpression(@"^[A-Z]{2}-[A-Z]{2}-\d{3}$", ErrorMessage = "A rendszámtábla kötelező formátuma: AA-AA-123")]
    public string? LicensePlate { get; set; } = null!;

    public int? BranchId { get; set; }

    public int? TransmissionId { get; set; }

    public int? FuelTypeId { get; set; }

    public virtual ICollection<AdditionalEquipment> AdditionalEquipments { get; set; } = new List<AdditionalEquipment>();

    public virtual Branch Branch { get; set; } = null!;

    public virtual FuelType FuelType { get; set; } = null!;

    public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();

    public virtual TransmissionType Transmission { get; set; } = null!;
}
