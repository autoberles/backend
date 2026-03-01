using System;
using System.Collections.Generic;

namespace autoberles_backend.Models;

public partial class AdditionalEquipment
{
    public int Id { get; set; }

    public int CarId { get; set; }

    public bool ParkingSensors { get; set; }

    public int AirConditioningId { get; set; }

    public bool HeatedSeats { get; set; }

    public bool Navigation { get; set; }

    public bool LeatherSeats { get; set; }

    public virtual AirConditioningType AirConditioning { get; set; } = null!;

    public virtual Car Car { get; set; } = null!;
}
