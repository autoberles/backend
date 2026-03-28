using FluentAssertions;
using autoberles_backend.Controllers;
using autoberles_backend.Models;
using autoberles_tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace autoberles_tests.Tests;

public class BranchTests
{
    [Fact(DisplayName = "[Branch] Should return all branches")]
    public async Task ReturnsAllBranches()
    {
        var context = TestDbContextFactory.Create();
        var controller = new BranchController(context);
        var actionResult = await controller.GetAllBranches();
        var okResult = actionResult as OkObjectResult;
        if (okResult == null)
            throw new Exception("No OkObjectResult was received from the API.");
        var data = okResult!.Value as List<Branch>;
        if (data == null)
            throw new Exception("The returned data is not a list.");
        data.Count.Should().Be(8, "there should be 8 elements");
    }


    [Fact(DisplayName = "[Branch] Should return branch by ID")]
    public async Task ReturnsBranchById()
    {
        var context = TestDbContextFactory.Create();
        var controller = new BranchController(context);
        var actionResult = await controller.GetBranchById(1);
        var okResult = actionResult as OkObjectResult;
        if (okResult == null)
            throw new Exception("No OkObjectResult was received from the API.");
        var branch = okResult!.Value as Branch;
        if (branch == null)
            throw new Exception("Expected the returned data to be a Branch.");
        branch.Id.Should().Be(1, "the returned object should have ID 1");
        branch.City.Should().Be("Budapest");
    }


    [Fact(DisplayName = "[Branch] Should create new branch")]
    public async Task CreatesBranch()
    {
        var context = TestDbContextFactory.Create();
        var controller = new BranchController(context);

        var newBranch = new Branch
        {
            City = "Eger",
            Address = "3300 Eger, Teszt utca 1.",
            Email = "eger@autoker.hu",
            PhoneNumber = "+36 30 123 4567"
        };

        var result = await controller.PostBranch(newBranch);
        Assert.IsType<CreatedAtActionResult>(result);
    }


    [Fact(DisplayName = "[Branch] Should patch branch city")]
    public async Task UpdatesCity()
    {
        var context = TestDbContextFactory.Create();
        var controller = new BranchController(context);

        var patchJson = JsonSerializer.Serialize(new
        {
            city = "Komárom"
        });

        var element = JsonSerializer.Deserialize<JsonElement>(patchJson);
        var result = await controller.PatchBranch(1, element);
        Assert.IsType<OkObjectResult>(result);
    }


    [Fact(DisplayName = "[Branch] Should delete branch")]
    public async Task RemovesBranch()
    {
        var context = TestDbContextFactory.Create();
        var controller = new BranchController(context);
        var result = await controller.DeleteBranch(8);
        Assert.IsType<OkObjectResult>(result);
    }


    [Fact(DisplayName = "[Branch] Should return NotFound for invalid ID")]
    public async Task ReturnsNotFoundForInvalidID()
    {
        var context = TestDbContextFactory.Create();
        var controller = new BranchController(context);
        var result = await controller.GetBranchById(999);
        Assert.IsType<NotFoundObjectResult>(result);
    }

}