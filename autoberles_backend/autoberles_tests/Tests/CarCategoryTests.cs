using FluentAssertions;
using autoberles_backend.Controllers;
using autoberles_backend.Models;
using autoberles_tests.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace autoberles_tests.Tests;

public class CarCategoryTests
{
    [Fact(DisplayName = "[CarCategory] Should return all car categories")]
    public async Task ReturnsAllCarCategories()
    {
        var context = TestDbContextFactory.Create();
        var controller = new CarCategoryController(context);
        var action = await controller.GetAllCarCategories();
        var ok = action as OkObjectResult;
        if (ok == null)
            throw new Exception("No OkObjectResult was received from the API.");
        var data = ok.Value as List<CarCategory>;
        if (data == null)
            throw new Exception("The returned data is not a list.");
        data.Count.Should().Be(5, "there should be 5 elements");
    }


    [Fact(DisplayName = "[CarCategory] Should return car category by ID")]
    public async Task ReturnsCarCategoryById()
    {
        var context = TestDbContextFactory.Create();
        var controller = new CarCategoryController(context);
        var action = await controller.GetCarCategoryById(1);
        var ok = action as OkObjectResult;
        if (ok == null)
            throw new Exception("No OkObjectResult was received from the API.");
        var data = ok.Value as CarCategory;
        if (data == null)
            throw new Exception("Expected the returned data to be a CarCategory.");
        data.Id.Should().Be(1, "the returned object should have ID 1");
        data.Name.Should().Be("gazdaságos kisautó", "the first value should be 'gazdaságos kisautó'");
    }
}