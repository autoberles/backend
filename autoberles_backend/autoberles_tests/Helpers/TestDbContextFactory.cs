using autoberles_backend;
using autoberles_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace autoberles_tests.Helpers;

public static class TestDbContextFactory
{
    public static CarRentalContext Create()
    {
        var options = new DbContextOptionsBuilder<CarRentalContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        var context = new CarRentalContext(options);
        Seed(context);
        return context;
    }

    private static void Seed(CarRentalContext context)
    {
        var airConditioningTypes = new List<AirConditioningType>
        {
            new AirConditioningType { Id = 1, Name = "automata" },
            new AirConditioningType { Id = 2, Name = "manuális" }
        };
        context.AirConditioningTypes.AddRange(airConditioningTypes);

        var cars = new List<Car>
        {
            new Car
            {
                Id = 1,
                Brand = "Test",
                Model = "Car1",
                Availability = true,
                Year = 2020,
                Color = "fekete",
                OwnWeight = 1000,
                TotalWeight = 1500,
                NumberOfSeats = 5,
                NumberOfDoors = 5,
                Price = 1000000,
                LicensePlate = "TE-ST-001",
                Mileage = 10000,
                LuggageCapacity = 300,
                CubicCapacity = 1600,
                PerformanceKw = 100,
                PerformanceHp = 130,
                LastServiceDate = DateTime.Now,
                InspectionExpiryDate = DateTime.Now.AddYears(1),
                BranchId = 1,
                TransmissionId = 1,
                FuelTypeId = 1,
                WheelDriveTypeId = 1,
                CarCategoryId = 1,
                DefaultPricePerDay = 10000,
                ImgUrl = "test1.jpg"
            },
            new Car
            {
                Id = 2,
                Brand = "Test",
                Model = "Car2",
                Availability = true,
                Year = 2021,
                Color = "fehér",
                OwnWeight = 1100,
                TotalWeight = 1600,
                NumberOfSeats = 5,
                NumberOfDoors = 5,
                Price = 1200000,
                LicensePlate = "TE-ST-002",
                Mileage = 20000,
                LuggageCapacity = 320,
                CubicCapacity = 1800,
                PerformanceKw = 110,
                PerformanceHp = 140,
                LastServiceDate = DateTime.Now,
                InspectionExpiryDate = DateTime.Now.AddYears(1),
                BranchId = 1,
                TransmissionId = 1,
                FuelTypeId = 1,
                WheelDriveTypeId = 1,
                CarCategoryId = 1,
                DefaultPricePerDay = 12000,
                ImgUrl = "test2.jpg"
            }
        };
        context.Cars.AddRange(cars);

        var equipments = new List<AdditionalEquipment>
        {
            new AdditionalEquipment
            {
                Id = 1,
                CarId = 1,
                ParkingSensors = true,
                AirConditioningId = 1,
                HeatedSeats = true,
                Navigation = true,
                LeatherSeats = false,
                Tempomat = true
            },
            new AdditionalEquipment
            {
                Id = 2,
                CarId = 2,
                ParkingSensors = false,
                AirConditioningId = 2,
                HeatedSeats = false,
                Navigation = true,
                LeatherSeats = false,
                Tempomat = false
            }
        };
        context.AdditionalEquipments.AddRange(equipments);

        var carCategories = new List<CarCategory>
        {
            new CarCategory { Id = 1, Name = "gazdaságos kisautó" },
            new CarCategory { Id = 2, Name = "középkategóriás családi autó" },
            new CarCategory { Id = 3, Name = "nagykategóriás autó" },
            new CarCategory { Id = 4, Name = "sportautó" },
            new CarCategory { Id = 5, Name = "városi terepjáró" }
        };
        context.CarCategories.AddRange(carCategories);

        var fuelTypes = new List<FuelType>
        {
            new FuelType { Id = 1, Name = "benzines" },
            new FuelType { Id = 2, Name = "dízel" },
            new FuelType { Id = 3, Name = "elektromos" },
            new FuelType { Id = 4, Name = "hibrid" }
        };
        context.FuelTypes.AddRange(fuelTypes);

        var transmissionTypes = new List<TransmissionType>
        {
            new TransmissionType { Id = 1, Name = "automata" },
            new TransmissionType { Id = 2, Name = "dupla kuplungos" },
            new TransmissionType { Id = 3, Name = "fokozatmentes automata" },
            new TransmissionType { Id = 4, Name = "manuális" }
        };
        context.TransmissionTypes.AddRange(transmissionTypes);

        var wheeldriveTypes = new List<WheelDriveType>
        {
            new WheelDriveType { Id = 1, Name = "elsőkerék-meghajtású" },
            new WheelDriveType { Id = 2, Name = "hátsókerék-meghajtású" },
            new WheelDriveType { Id = 3, Name = "összkerékmeghajtású" }
        };
        context.WheelDriveTypes.AddRange(wheeldriveTypes);

        context.SaveChanges();
    }
}