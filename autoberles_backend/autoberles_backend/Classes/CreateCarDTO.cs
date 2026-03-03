namespace autoberles_backend.Classes
{
    public class CreateCarDTO
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public short NumberOfSeats { get; set; }
        public int Price { get; set; }
        public string LicensePlate { get; set; }

        public int BranchId { get; set; }
        public int TransmissionId { get; set; }
        public int FuelTypeId { get; set; }

        public CreateAdditionalEquipmentDto AdditionalEquipment { get; set; }
    }

    public class CreateAdditionalEquipmentDto
    {
        public bool ParkingSensors { get; set; }
        public bool HeatedSeats { get; set; }
        public bool Navigation { get; set; }
        public bool LeatherSeats { get; set; }
        public int AirConditioningId { get; set; }
    }
}
