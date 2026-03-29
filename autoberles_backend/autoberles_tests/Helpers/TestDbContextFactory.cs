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

    public static CarRentalContext CreateEmpty()
    {
        var options = new DbContextOptionsBuilder<CarRentalContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        return new CarRentalContext(options);
    }

    private static void Seed(CarRentalContext context)
    {
        var airConditioningTypes = new List<AirConditioningType>
        {
            new AirConditioningType { Id = 1, Name = "automata" },
            new AirConditioningType { Id = 2, Name = "manuális" }
        };
        context.AirConditioningTypes.AddRange(airConditioningTypes);

        var branches = new List<Branch>
        {
            new Branch
            {
                Id = 1,
                City = "Budapest",
                Address = "1044 Budapest, Váci út 123.",
                Email = "budapest@autoker.hu",
                PhoneNumber = "+36 15 555 1231"
            },
            new Branch
            {
                Id = 2,
                City = "Győr",
                Address = "9022 Győr, Szent István út 45.",
                Email = "gyor@autoker.hu",
                PhoneNumber = "+36 96 555 6781"
            },
            new Branch
            {
                Id = 3,
                City = "Szeged",
                Address = "6724 Szeged, Kossuth Lajos sugárút 78.",
                Email = "szeged@autoker.hu",
                PhoneNumber = "+36 62 555 9871"
            },
            new Branch
            {
                Id = 4,
                City = "Debrecen",
                Address = "4026 Debrecen, Piac utca 12.",
                Email = "debrecen@autoker.hu",
                PhoneNumber = "+36 52 555 1111"
            },
            new Branch
            {
                Id = 5,
                City = "Székesfehérvár",
                Address = "8000 Székesfehérvár, Palotai út 33.",
                Email = "fehervar@autoker.hu",
                PhoneNumber = "+36 22 555 2221"
            },
            new Branch
            {
                Id = 6,
                City = "Pécs",
                Address = "7621 Pécs, Rákóczi út 50.",
                Email = "pecs@autoker.hu",
                PhoneNumber = "+36 72 555 3331"
            },
            new Branch
            {
                Id = 7,
                City = "Miskolc",
                Address = "3711 Miskolc, Széchenyi utca 88.",
                Email = "miskolc@autoker.hu",
                PhoneNumber = "+36 46 555 4441"
            },
            new Branch
            {
                Id = 8,
                City = "Zalaegerszeg",
                Address = "8900 Zalaegerszeg, Kossuth Lajos utca 21.",
                Email = "zalaegerszeg@autoker.hu",
                PhoneNumber = "+36 92 555 5551"
            }
        };
        context.Branches.AddRange(branches);

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

        var users = new List<User>
        {
            new User
            {
                Id = 1,
                FirstName = "Admin",
                LastName = "User",
                Email = "admin@test.hu",
                PhoneNumber = "+36 20 123 4567",
                BirthDate = new DateTime(1990, 1, 1),
                Role = "admin",
                PasswordHash = "hash"
            },
            new User
            {
                Id = 2,
                FirstName = "Agent",
                LastName = "User",
                Email = "agent@test.hu",
                PhoneNumber = "+36 20 123 4568",
                BirthDate = new DateTime(1992, 1, 1),
                Role = "agent",
                PasswordHash = "hash"
            },
            new User
            {
                Id = 3,
                FirstName = "Customer",
                LastName = "User",
                Email = "customer@test.hu",
                PhoneNumber = "+36 20 123 4569",
                BirthDate = new DateTime(1995, 1, 1),
                Role = "customer",
                PasswordHash = "hash"
            }
        };
        context.Users.AddRange(users);

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