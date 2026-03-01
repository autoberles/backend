using System;
using System.Collections.Generic;

namespace autoberles_backend.Models;

public partial class Car
{
    public int Id { get; set; }

    public string Brand { get; set; } = null!;

    public string Model { get; set; } = null!;

    public int BranchId { get; set; }

    public int Year { get; set; }

    public int TransmissionId { get; set; }

    public int FuelTypeId { get; set; }

    public sbyte NumberOfSeats { get; set; }

    public int Price { get; set; }

    public virtual ICollection<AdditionalEquipment> AdditionalEquipments { get; set; } = new List<AdditionalEquipment>();

    public virtual Branch Branch { get; set; } = null!;

    public virtual FuelType FuelType { get; set; } = null!;

    public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();

    public virtual TransmissionType Transmission { get; set; } = null!;
}
