using FluentAssertions;
using autoberles_backend.Controllers;
using autoberles_backend.Models;
using autoberles_tests.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace autoberles_tests.Tests;

public class TransmissionTypeTests
{
    [Fact(DisplayName = "[TransmissionType] Should return all transmission types")]
    public async Task ReturnsAllTransmissionTypes()
    {
        var context = TestDbContextFactory.Create();
        var controller = new TransmissionTypeController(context);
        var actionResult = await controller.GetAllTransmissionTypes();
        var okResult = actionResult as OkObjectResult;
        if (okResult == null)
            throw new Exception("No OkObjectResult was received from the API.");
        var data = okResult.Value as List<TransmissionType>;
        if (data == null)
            throw new Exception("The returned data is not a list.");
        data.Count.Should().Be(4, "there should be 4 elements");
    }

    [Fact(DisplayName = "[TransmissionType] Should return transmission type by ID")]
    public async Task ReturnsFuelTypeById()
    {
        var context = TestDbContextFactory.Create();
        var controller = new TransmissionTypeController(context);
        var actionResult = await controller.GetTransmissionTypeById(1);
        var okResult = actionResult as OkObjectResult;
        if (okResult == null)
            throw new Exception("No OkObjectResult was received from the API.");
        var data = okResult.Value as TransmissionType;
        if (data == null)
            throw new Exception("Expected the returned data to be a TransmissionType.");
        data.Id.Should().Be(1, "the returned object should have ID 1");
        data.Name.Should().Be("automata", "the first value should be 'automata'");
    }
}