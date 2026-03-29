using FluentAssertions;
using autoberles_backend.Controllers;
using autoberles_backend.Models;
using autoberles_tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace autoberles_tests.Tests;

public class CarTests
{
    [Fact(DisplayName = "[Car] Should return all cars with additional equipments")]
    public async Task ReturnsAllCars()
    {
        var context = TestDbContextFactory.Create();
        var controller = new CarController(context);
        var actionResult = await controller.GetAllCars();
        var okResult = actionResult as OkObjectResult;
        if (okResult == null)
            throw new Exception("No OkObjectResult was received from the API.");
        var cars = okResult!.Value as List<Car>;
        if (cars == null)
            throw new Exception("The returned data is not a list."); 
        cars!.Count.Should().Be(2, "there should be 2 elements");
        if (cars[0].AdditionalEquipment == null)
            throw new Exception("The returned data is not a list.");
    }


    [Fact(DisplayName = "[Car] Should return car by ID")]
    public async Task ReturnsCarById()
    {
        var context = TestDbContextFactory.Create();
        var controller = new CarController(context);
        var actionResult = await controller.GetCarById(1);
        var okResult = actionResult as OkObjectResult;
        if (okResult == null)
            throw new Exception("No OkObjectResult was received from the API.");
        var car = okResult!.Value as Car;
        if (car == null)
            throw new Exception("Expected the returned data to be a Car.");
        car!.Id.Should().Be(1, "the returned object should have ID 1");
        car.Brand.Should().Be("Test");
    }


    [Fact(DisplayName = "[Car] Should return NotFound for invalid ID")]
    public async Task ReturnsNotFoundForInvalidID()
    {
        var context = TestDbContextFactory.Create();
        var controller = new CarController(context);
        var result = await controller.GetCarById(999);
        Assert.IsType<NotFoundObjectResult>(result);
    }


    [Fact(DisplayName = "[Car] Should create new car")]
    public async Task CreatesCar()
    {
        var context = TestDbContextFactory.Create();
        var controller = new CarController(context);

        var car = new Car
        {
            Availability = true,
            Brand = "BMW",
            Model = "X5",
            Year = 2022,
            Color = "fekete",
            OwnWeight = 2000,
            TotalWeight = 2500,
            NumberOfSeats = 5,
            NumberOfDoors = 5,
            Price = 15000000,
            LicensePlate = "AA-BB-123",
            Mileage = 10000,
            LuggageCapacity = 500,
            CubicCapacity = 3000,
            PerformanceKw = 200,
            PerformanceHp = 300,
            LastServiceDate = DateTime.Now,
            InspectionExpiryDate = DateTime.Now.AddYears(1),
            BranchId = 1,
            TransmissionId = 1,
            FuelTypeId = 1,
            WheelDriveTypeId = 1,
            CarCategoryId = 1,
            DefaultPricePerDay = 20000,
            ImgUrl = "test.jpg",
            AdditionalEquipment = new AdditionalEquipment
            {
                ParkingSensors = true,
                AirConditioningId = 1,
                HeatedSeats = true,
                Navigation = true,
                LeatherSeats = true,
                Tempomat = true
            }
        };

        var result = await controller.PostCar(car);
        Assert.IsType<CreatedAtActionResult>(result);
    }


    [Fact(DisplayName = "[Car] Should update car price")]
    public async Task UpdatesPrice()
    {
        var context = TestDbContextFactory.Create();
        var controller = new CarController(context);

        var json = JsonSerializer.Serialize(new
        {
            price = 12000000
        });

        var element = JsonSerializer.Deserialize<JsonElement>(json);
        var result = await controller.PatchCar(1, element);
        Assert.IsType<OkObjectResult>(result);
    }


    [Fact(DisplayName = "[Car] Should delete car")]
    public async Task RemovesCar()
    {
        var context = TestDbContextFactory.Create();
        var controller = new CarController(context);
        var result = await controller.DeleteCar(1);
        Assert.IsType<OkObjectResult>(result);
    }


    [Fact(DisplayName = "[Car] Should fail on invalid JSON patch")]
    public async Task PatchCar_InvalidJson_ReturnsBadRequest()
    {
        var context = TestDbContextFactory.Create();
        var controller = new CarController(context);
        var json = JsonSerializer.Serialize("invalid");
        var element = JsonSerializer.Deserialize<JsonElement>(json);
        var result = await controller.PatchCar(1, element);
        Assert.IsType<BadRequestObjectResult>(result);

    }

}