using autoberles_backend.Classes;
using autoberles_backend.Controllers;
using autoberles_backend.Models;
using autoberles_tests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace autoberles_tests.Tests
{
    public class UserControllerTests
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


        [Fact(DisplayName = "[User] Should return all users")]
        public async Task ReturnsAllUsers()
        {
            var context = TestDbContextFactory.Create();
            var controller = new UserController(context);
            SetUser(controller, "admin");
            var action = await controller.GetAllUsers();
            var ok = action as OkObjectResult;
            if (ok == null)
                throw new Exception("No OkObjectResult was received from the API.");
            var users = ok.Value as List<User>;
            if (users == null)
                throw new Exception("The returned data is not a list.");
            users.Count().Should().Be(3, "there should be 3 elements");
        }


        [Fact(DisplayName = "[User] Should return user by ID")]
        public async Task ReturnsUserById()
        {
            var context = TestDbContextFactory.Create();
            var controller = new UserController(context);
            SetUser(controller, "admin");
            var action = await controller.GetUserById(1);
            var ok = action as OkObjectResult;
            if (ok == null)
                throw new Exception("No OkObjectResult was received from the API.");
            var user = ok.Value as User;
            if (user == null)
                throw new Exception("Expected the returned data to be a User.");
            user.Id.Should().Be(1, "the returned object should have ID 1");
            user.Email.Should().Be("admin@test.hu");
        }


        [Fact(DisplayName = "[User] Should return all customers")]
        public async Task ReturnsAllCustomers()
        {
            var context = TestDbContextFactory.Create();
            var controller = new UserController(context);
            SetUser(controller, "agent");
            var action = await controller.GetAllCustomers();
            var ok = action as OkObjectResult;
            if (ok == null)
                throw new Exception("No OkObjectResult was received from the API.");
            var customer = ok.Value as List<User>;
            if (customer == null)
                throw new Exception("Expected the returned data to be a customer.");
            customer.Should().OnlyContain(x => x.Role == "customer");
        }


        [Fact(DisplayName = "[User] Should create user")]
        public async Task CreatesUser()
        {
            var context = TestDbContextFactory.CreateEmpty();
            var controller = new UserController(context);
            SetUser(controller, "admin");

            var request = new CreateUser
            {
                FirstName = "Test",
                LastName = "User",
                Email = "new@test.hu",
                PhoneNumber = "+36 20 999 9999",
                BirthDate = new DateTime(1995, 1, 1),
                Role = "customer",
                Password = "123456",
                ConfirmPassword = "123456"
            };

            var action = await controller.CreateUser(request);
            Assert.IsType<OkObjectResult>(action);
        }


        [Fact(DisplayName = "[User] Should delete user")]
        public async Task RemovesUser()
        {
            var context = TestDbContextFactory.Create();

            var user = new User
            {
                Id = 100,
                FirstName = "Test",
                LastName = "User",
                Email = "delete@test.hu",
                PhoneNumber = "+36 20 111 1111",
                BirthDate = DateTime.Now,
                Role = "customer",
                PasswordHash = "x"
            };

            context.Users.Add(user);
            context.SaveChanges();
            var controller = new UserController(context);
            SetUser(controller, "admin", 1);
            var action = await controller.DeleteUser(user.Id);
            Assert.IsType<OkObjectResult>(action);
            var deletedUser = await context.Users.FindAsync(user.Id);
            if (deletedUser != null)
                throw new Exception("Expected the returned data to be null.");
        }


        [Fact(DisplayName = "[User] User should not delete itself")]
        public async Task UserCanNotRemoveItself()
        {
            var context = TestDbContextFactory.Create();
            var controller = new UserController(context);
            SetUser(controller, "admin", 1);
            var action = await controller.DeleteUser(1);
            Assert.IsType<BadRequestObjectResult>(action);
        }
    }
}
