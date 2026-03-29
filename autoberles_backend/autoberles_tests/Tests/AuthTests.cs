using autoberles_backend.Controllers;
using autoberles_backend;
using autoberles_tests.Helpers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using autoberles_backend.Classes;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using autoberles_backend.Models;

namespace autoberles_tests.Tests
{
    public class AuthTests
    {
        private AuthController CreateController(CarRentalContext context)
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                {"Jwt:Key", "super_secret_test_key_12345_very_long_key_123"},
                {"Jwt:Issuer", "test"},
                {"Jwt:Audience", "test"}
                }).Build();

            var emailService = new FakeEmailService();
            return new AuthController(context, config, emailService);
        }


        [Fact(DisplayName = "[Auth] Should register a new user")]
        public async Task RegisterSuccess()
        {
            var context = TestDbContextFactory.CreateEmpty();
            var controller = CreateController(context);

            var register = new Register
            {
                Email = "test@test.com",
                Password = "1234",
                FirstName = "Test",
                LastName = "User",
                PhoneNumber = "123456789",
                BirthDate = DateTime.Now
            };

            var result = await controller.Register(register);
            var okResult = Assert.IsType<OkObjectResult>(result);
            context.Users.Count().Should().Be(1, "one user should be created");
        }


        [Fact(DisplayName = "[Auth] Should not register with existing email")]
        public async Task RegisterFailsWhenEmailExists()
        {
            var context = TestDbContextFactory.CreateEmpty();

            context.Users.Add(new User
            {
                Email = "test@test.com",
                PhoneNumber = "111111111",
                PasswordHash = "x",
                FirstName = "Test",
                LastName = "User",
                BirthDate = DateTime.Now,
                Role = "customer"
            });

            context.SaveChanges();
            var controller = CreateController(context);

            var register = new Register
            {
                Email = "test@test.com",
                Password = "1234",
                FirstName = "Test",
                LastName = "User",
                PhoneNumber = "123456789",
                BirthDate = DateTime.Now
            };

            var result = await controller.Register(register);
            Assert.IsType<BadRequestObjectResult>(result);
        }


        [Fact(DisplayName = "[Auth] Should login with correct credentials")]
        public async Task LoginSuccess()
        {
            var context = TestDbContextFactory.CreateEmpty();

            var user = new User
            {
                Email = "test@test.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("1234"),
                Role = "customer",
                FirstName = "Test",
                LastName = "User",
                PhoneNumber = "123456789",
                BirthDate = DateTime.Now
            };

            context.Users.Add(user);
            context.SaveChanges();
            var controller = CreateController(context);

            var login = new Login
            {
                Email = "test@test.com",
                Password = "1234"
            };

            var result = await controller.Login(login);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }


        [Fact(DisplayName = "[Auth] Should reset password with valid code")]
        public async Task ResetPasswordSuccess()
        {
            var context = TestDbContextFactory.CreateEmpty();

            context.Users.Add(new User
            {
                FirstName = "Test",
                LastName = "User",
                PhoneNumber = "123456789",
                Email = "test@test.com",
                PasswordHash = "old",
                ResetToken = "123456",
                ResetTokenExpiry = DateTime.UtcNow.AddMinutes(10)
            });

            context.SaveChanges();
            var controller = CreateController(context);

            var reset = new ResetPassword
            {
                Email = "test@test.com",
                Code = "123456",
                NewPassword = "newpass"
            };

            var result = await controller.ResetPassword(reset);
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
