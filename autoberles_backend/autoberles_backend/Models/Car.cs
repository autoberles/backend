using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace autoberles_backend.Models;


[Index(nameof(LicensePlate), IsUnique = true)]
public partial class Car
{
    [Key]
    public int Id { get; set; }


    [Required(ErrorMessage = "Kötelező megadni, hogy az autó jelenleg bérelve van-e.")]
    public bool Availability { get; set; }


    [Required(ErrorMessage = "Az autó márkájának megadása kötelező.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "A márka nevének hossza 2 és 50 karakter közötti legyen.")]
    public string Brand { get; set; } = null!;


    [Required(ErrorMessage = "Az autó modellének megadása kötelező.")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "A modell nevének hossza 1 és 50 karakter közötti legyen.")]
    public string Model { get; set; } = null!;


    [Required(ErrorMessage = "Az évszám megadása kötelező.")]
    [Range(1980, 2100, ErrorMessage = "Az évszámnak 1980 és 2100 közöttinek kell lennie.")]
    public int Year { get; set; }


    [Required(ErrorMessage = "Az autó színének megadása kötelező.")]
    public string Color { get; set; } = null!;


    [Required(ErrorMessage = "Az autó nettó tömegének megadása kötelező.")]
    public int OwnWeight { get; set; }


    [Required(ErrorMessage = "Az autó megengedett legnagyobb tömegének megadása kötelező.")]
    public int TotalWeight { get; set; }


    [Required(ErrorMessage = "Az ülések darabszámának megadása kötelező.")]
    [Range(2, 9, ErrorMessage = "A darabszámnak 2 és 9 közöttinek kell lennie.")]
    public short? NumberOfSeats { get; set; }


    [Required(ErrorMessage = "Az ajtók darabszámának megadása kötelező.")]
    [Range(2, 5, ErrorMessage = "A darabszámnak 2 és 5 közöttinek kell lennie.")]
    public short? NumberOfDoors { get; set; }


    [Required(ErrorMessage = "Az autó árának megadása kötelező.")]
    public int? Price { get; set; }


    [Required(ErrorMessage = "Az autó rendszámának megadása kötelező.")]
    [RegularExpression(@"^[A-Z]{2}-[A-Z]{2}-\d{3}$", ErrorMessage = "A rendszámtábla kötelező formátuma: AA-AA-123")]
    public string LicensePlate { get; set; } = null!;


    [Required(ErrorMessage = "Az autó megtett kilométereinek számát kötelező megadni.")]
    public int? Mileage { get; set; }


    [Required(ErrorMessage = "Az autó csomagterének űrtartalmát kötelező megadni.")]
    public int? LuggageCapacity { get; set; }


    [Required(ErrorMessage = "Az autó hengerűrtartalmának megadása kötelező.")]
    public int CubicCapacity { get; set; }

    public int? TankCapacity { get; set; }

    public int? BatteryCapacity { get; set; }


    [Required(ErrorMessage = "A motor teljesítményét kilowattban megadni kötelező.")]
    public int PerformanceKw { get; set; }


    [Required(ErrorMessage = "A motor teljesítményét lóerőben megadni kötelező.")]
    public int PerformanceHp { get; set; }


    [Required(ErrorMessage = "Az utolsó szervízi látogatás időpontját kötelező megadni.")]
    [DataType(DataType.Date)]
    public DateTime? LastServiceDate { get; set; }


    [Required(ErrorMessage = "A műszaki vizsga lejárati dátumát kötelező megadni.")]
    [DataType(DataType.Date)]
    public DateTime? InspectionExpiryDate { get; set; }

    public int BranchId { get; set; }

    public int TransmissionId { get; set; }

    public int FuelTypeId { get; set; }

    public int WheelDriveTypeId { get; set; }

    public int CarCategoryId { get; set; }

    public int DefaultPricePerDay { get; set; }

    public virtual AdditionalEquipment? AdditionalEquipment { get; set; } = null!;

    [JsonIgnore]
    public virtual Branch? Branch { get; set; } = null!;

    [JsonIgnore]
    public virtual CarCategory? CarCategory { get; set; } = null!;

    [JsonIgnore]
    public virtual FuelType? FuelType { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();

    [JsonIgnore]
    public virtual TransmissionType? TransmissionType { get; set; } = null!;

    [JsonIgnore]
    public virtual WheelDriveType? WheelDriveType { get; set; } = null!;
}
