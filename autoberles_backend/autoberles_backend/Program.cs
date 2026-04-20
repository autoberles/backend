using autoberles_backend.Classes;
using autoberles_backend.Models;
using autoberles_backend.Services;
using BCrypt.Net;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using QuestPDF.Infrastructure;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json;

namespace autoberles_backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<CarRentalContext>(options =>
                options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                    options.JsonSerializerOptions.Converters.Add(new NullableDateTimeConverter());
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
                });

            builder.Services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.Cookie.Name = "auth_cookie";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SameSite = SameSiteMode.Lax;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                });

            builder.Services.AddAuthorization();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("http://localhost:5173")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(AddSwaggerAuthBtn);
            builder.Services.AddScoped<EmailService>();

            QuestPDF.Settings.License = LicenseType.Community;

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<CarRentalContext>();

                if (!context.Users.Any(u => u.Role == "admin"))
                {
                    var adminHash = BCrypt.Net.BCrypt.HashPassword("Tothpista1");
                    var admin = new User
                    {
                        LastName = "Tóth",
                        FirstName = "Pista",
                        Email = "tothpista@admin.hu",
                        PasswordHash = adminHash,
                        PhoneNumber = "+36 20 780 1935",
                        BirthDate = new DateTime(1990, 5, 25),
                        Role = "admin"
                    };
                    context.Users.Add(admin);
                }

                if (!context.Users.Any(u => u.Role == "agent"))
                {
                    var agentHash = BCrypt.Net.BCrypt.HashPassword("Kislili1");
                    var agent = new User
                    {
                        LastName = "Kis",
                        FirstName = "Lili",
                        Email = "kislili@agent.com",
                        PasswordHash = agentHash,
                        PhoneNumber = "+36 20 516 4053",
                        BirthDate = new DateTime(1992, 7, 3),
                        Role = "agent"
                    };
                    context.Users.Add(agent);
                }

                context.SaveChanges();
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowFrontend");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();

            app.MapControllers();

            app.Run();
        }

        private static void AddSwaggerAuthBtn(SwaggerGenOptions options)
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Auth Demo API",
                Version = "v1"
            });
        }
    }
}