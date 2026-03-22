using autoberles_backend.Classes;
using autoberles_backend.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using System.Text.Json;
using BCrypt.Net;
using autoberles_backend.Services;

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

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(AddSwaggerAuthBtn);
            builder.Services.AddScoped<EmailService>();

            var jwtSettings = builder.Configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

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
                        PhoneNumber = "+36 20 780 193",
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
                        PhoneNumber = "+36 20 516 403",
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static void AddSwaggerAuthBtn(SwaggerGenOptions options)
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "JWT Demo API",
                Version = "v1"
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        }
    }
}