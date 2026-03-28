using FluentAssertions;
using autoberles_backend.Controllers;
using autoberles_backend.Models;
using autoberles_tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace autoberles_tests.Tests;

public class AdditionalEquipmentTests
{

    [Fact(DisplayName = "[AdditionalEquipment] Should return all additional equipments")]
    public async Task ReturnsAllAdditionalEquipments()
    {
        var context = TestDbContextFactory.Create();
        var controller = new AdditionalEquipmentController(context);
        var actionResult = await controller.GetAllAdditionalEquipments();
        var okResult = actionResult as OkObjectResult;
        if (okResult == null)
            throw new Exception("No OkObjectResult was received from the API.");
        var data = okResult.Value as List<AdditionalEquipment>;
        if (data == null)
            throw new Exception("The returned data is not a list.");
        data.Count.Should().Be(2, "there must be 2 elements.");
    }


    [Fact(DisplayName = "[AdditionalEquipment] Should return additional equipment by ID")]
    public async Task ReturnsAdditionalEquipmentById()
    {
        var context = TestDbContextFactory.Create();
        var controller = new AdditionalEquipmentController(context);
        var actionResult = await controller.GetAdditionalEquipmentsById(1);
        var okResult = actionResult as OkObjectResult;
        if (okResult == null)
            throw new Exception("No OkObjectResult was received from the API.");
        var data = okResult.Value as AdditionalEquipment;
        if (data == null)
            throw new Exception("The returned data is not an AdditionalEquipment object.");
        data.Id.Should().Be(1, "the returned object should have ID 1");
        data.CarId.Should().Be(1, "the returned object should belong to CarId 1");
    }


    [Fact(DisplayName = "[AdditionalEquipment] Should patch an existing additional equipment")]
    public async Task UpdatesExistingAdditionalEquipment()
    {
        var context = TestDbContextFactory.Create();
        var controller = new AdditionalEquipmentController(context);
        var patchBody = new
        {
            navigation = true
        };
        var jsonBody = JsonSerializer.Serialize(patchBody);
        var jsonElement = JsonSerializer.Deserialize<JsonElement>(jsonBody);
        var actionResult = await controller.PatchAdditionalEquipment(1, jsonElement);
        var okResult = actionResult as OkObjectResult;
        if (okResult == null)
            throw new Exception("No OkObjectResult was returned from the API.");
        ((string)okResult.Value!).Should().Be("A(z) 1 ID-val rendelkező felszereltség frissítésre került.");
        var updated = await context.AdditionalEquipments.FindAsync(1);
        updated!.Navigation.Should().BeTrue("the Navigation field needed to be updated");
    }
}