using FluentAssertions;
using autoberles_backend.Controllers;
using autoberles_backend.Models;
using autoberles_tests.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace autoberles_tests.Tests;

public class AirConditioningTypeTests
{

    [Fact(DisplayName = "[AirConditioningType] Should return all air conditioning types")]
    public async Task ReturnsAllAirConditioningTypes()
    {
        var context = TestDbContextFactory.Create();
        var controller = new AirConditioningTypeController(context);
        var actionResult = await controller.GetAllAirConditioningTypes();
        var okResult = actionResult as OkObjectResult;
        if (okResult == null)
            throw new Exception("No OkObjectResult was received from the API.");
        var data = okResult.Value as List<AirConditioningType>;
        if (data == null)
            throw new Exception("The returned data is not a list.");
        data.Count.Should().Be(2, "there must be 2 elements.");
    }


    [Fact(DisplayName = "[AirConditioningType] Should return air conditioning type by ID")]
    public async Task ReturnsAirConditioningTypeById()
    {
        var context = TestDbContextFactory.Create();
        var controller = new AirConditioningTypeController(context);
        var actionResult = await controller.GetAirConditionalTypeById(1);
        var okResult = actionResult as OkObjectResult;
        if (okResult == null)
            throw new Exception("No OkObjectResult was received from the API.");
        var data = okResult.Value as AirConditioningType;
        if (data == null)
            throw new Exception("Expected the returned data to be an AirConditioningType.");
        data.Id.Should().Be(1, "the returned object should have ID 1");
        data.Name.Should().Be("automata", "the first value should be 'automata'");
    }
}