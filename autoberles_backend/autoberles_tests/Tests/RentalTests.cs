using autoberles_backend.Controllers;
using autoberles_backend.Models;
using autoberles_tests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace autoberles_tests.Tests;

public class RentalTests
{
    private void SetUser(ControllerBase controller, string role, int userId = 1)
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Role, role),
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        }, "mock"));

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }


    [Fact(DisplayName = "[Rental] Should return all rentals")]
    public async Task ReturnsAllRentals()
    {
        var context = TestDbContextFactory.Create();
        var controller = new RentalController(context, new FakeEmailService());
        SetUser(controller, "admin");
        var action = await controller.GetAllRentals();
        var okResult = Assert.IsType<OkObjectResult>(action);
        var rentals = Assert.IsType<List<Rental>>(okResult.Value);
        rentals.Count().Should().Be(1, "there should be 1 element");
    }


    [Fact(DisplayName = "[Rental] Should return rental by ID")]
    public async Task ReturnsRentalById()
    {
        var context = TestDbContextFactory.Create();
        var controller = new RentalController(context, new FakeEmailService());
        SetUser(controller, "admin");
        var action = await controller.GetRentalById(1);
        var okResult = Assert.IsType<OkObjectResult>(action);
        var rental = Assert.IsType<Rental>(okResult.Value);
        rental.Id.Should().Be(1, "the returned object should have ID 1");
    }


    [Fact(DisplayName = "[Rental] Should not create rental with invalid dates")]
    public async Task FailsToCreateWithInvalidDates()
    {
        var context = TestDbContextFactory.Create();
        var controller = new RentalController(context, new FakeEmailService());
        SetUser(controller, "customer", 1);

        var rental = new Rental
        {
            CarId = 1,
            StartDate = DateTime.Today.AddDays(5),
            EndDate = DateTime.Today.AddDays(3)
        };

        var action = await controller.PostRental(rental);
        Assert.IsType<BadRequestObjectResult>(action);
    }


    [Fact(DisplayName = "[Rental] Should patch rental")]
    public async Task UpdatesRental()
    {
        var context = TestDbContextFactory.Create();
        var controller = new RentalController(context, new FakeEmailService());
        SetUser(controller, "admin");

        var json = JsonSerializer.SerializeToElement(new
        {
            end_date = DateTime.Today.AddDays(5)
        });

        var action = await controller.PatchRental(1, json);
        Assert.IsType<OkObjectResult>(action);
    }


    [Fact(DisplayName = "[Rental] Should delete rental")]
    public async Task RemovesRental()
    {
        var context = TestDbContextFactory.Create();
        var controller = new RentalController(context, new FakeEmailService());
        SetUser(controller, "admin");
        var result = await controller.DeleteRental(1);
        Assert.IsType<OkObjectResult>(result);
        var rental = await context.Rentals.FindAsync(1);
        if (rental != null)
            throw new Exception("Expected the returned data to be null.");
    }


    [Fact(DisplayName = "[Rental] Should get my rentals")]
    public async Task ReturnsMyRentals()
    {
        var context = TestDbContextFactory.Create();
        var controller = new RentalController(context, new FakeEmailService());
        SetUser(controller, "customer", 1);
        var action = await controller.GetMyRentals();
        var ok = Assert.IsType<OkObjectResult>(action);
        var rentals = Assert.IsType<List<Rental>>(ok.Value);
        rentals.Should().OnlyContain(x => x.UserId == 1);
    }
}