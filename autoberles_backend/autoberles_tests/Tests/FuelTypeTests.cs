using FluentAssertions;
using autoberles_backend.Controllers;
using autoberles_backend.Models;
using autoberles_tests.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace autoberles_tests.Tests;

public class FuelTypeTests
{
    [Fact(DisplayName = "[FuelType] Should return all fuel types")]
    public async Task ReturnsAllFuelTypes()
    {
        var context = TestDbContextFactory.Create();
        var controller = new FuelTypeController(context);
        var action = await controller.GetAllFuelTypes();
        var ok = action as OkObjectResult;
        if (ok == null)
            throw new Exception("No OkObjectResult was received from the API.");
        var data = ok.Value as List<FuelType>;
        if (data == null)
            throw new Exception("The returned data is not a list.");
        data.Count.Should().Be(4, "there should be 4 elements");
    }


    [Fact(DisplayName = "[FuelType] Should return fuel type by ID")]
    public async Task ReturnsFuelTypeById()
    {
        var context = TestDbContextFactory.Create();
        var controller = new FuelTypeController(context);
        var action = await controller.GetFuelTypeById(1);
        var ok = action as OkObjectResult;
        if (ok == null)
            throw new Exception("No OkObjectResult was received from the API.");
        var data = ok.Value as FuelType;
        if (data == null)
            throw new Exception("Expected the returned data to be a FuelType.");
        data.Id.Should().Be(1, "the returned object should have ID 1");
        data.Name.Should().Be("benzines", "the first value should be 'benzines'");
    }
}