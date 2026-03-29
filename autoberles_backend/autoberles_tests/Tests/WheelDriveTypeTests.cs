using FluentAssertions;
using autoberles_backend.Controllers;
using autoberles_backend.Models;
using autoberles_tests.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace autoberles_tests.Tests;

public class WheelDriveTypeTests
{
    [Fact(DisplayName = "[WheelDriveType] Should return all wheel drive types")]
    public async Task ReturnsAllWheelDriveTypes()
    {
        var context = TestDbContextFactory.Create();
        var controller = new WheelDriveTypeController(context);
        var action = await controller.GetAllWheelDriveTypes();
        var ok = action as OkObjectResult;
        if (ok == null)
            throw new Exception("No OkObjectResult was received from the API.");
        var data = ok.Value as List<WheelDriveType>;
        if (data == null)
            throw new Exception("The returned data is not a list.");
        data.Count.Should().Be(3, "there should be 3 elements");
    }


    [Fact(DisplayName = "[WheelDriveType] Should return wheel drive type by ID")]
    public async Task ReturnsWheelDriveTypeById()
    {
        var context = TestDbContextFactory.Create();
        var controller = new WheelDriveTypeController(context);
        var action = await controller.GetWheelDriveTypeById(1);
        var ok = action as OkObjectResult;
        if (ok == null)
            throw new Exception("No OkObjectResult was received from the API.");
        var data = ok.Value as WheelDriveType;
        if (data == null)
            throw new Exception("Expected the returned data to be a WheelDriveType.");
        data.Id.Should().Be(1, "the returned object should have ID 1");
        data.Name.Should().Be("elsőkerék-meghajtású", "the first value should be 'elsőkerék-meghajtású'");
    }
}