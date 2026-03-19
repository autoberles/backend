using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using autoberles_backend.Models;

namespace autoberles_backend;

public partial class CarRentalContext : DbContext
{
    public CarRentalContext()
    {
    }

    public CarRentalContext(DbContextOptions<CarRentalContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AdditionalEquipment> AdditionalEquipments { get; set; }

    public virtual DbSet<AirConditioningType> AirConditioningTypes { get; set; }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<CarCategory> CarCategories { get; set; }

    public virtual DbSet<FuelType> FuelTypes { get; set; }

    public virtual DbSet<Rental> Rentals { get; set; }

    public virtual DbSet<TransmissionType> TransmissionTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WheelDriveType> WheelDriveTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySQL("Server=localhost;Database=car_rental;uid=root;pwd=;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdditionalEquipment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("additional_equipment");

            entity.HasIndex(e => e.AirConditioningId, "air_conditioning_id");

            entity.HasIndex(e => e.CarId, "car_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.AirConditioningId)
                .HasColumnType("int(11)")
                .HasColumnName("air_conditioning_id");
            entity.Property(e => e.CarId)
                .HasColumnType("int(11)")
                .HasColumnName("car_id");
            entity.Property(e => e.HeatedSeats).HasColumnName("heated_seats");
            entity.Property(e => e.LeatherSeats).HasColumnName("leather_seats");
            entity.Property(e => e.Navigation).HasColumnName("navigation");
            entity.Property(e => e.ParkingSensors).HasColumnName("parking_sensors");
            entity.Property(e => e.Tempomat).HasColumnName("tempomat");

            entity.HasOne(d => d.AirConditioning).WithMany(p => p.AdditionalEquipments)
                .HasForeignKey(d => d.AirConditioningId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.Car).WithOne(p => p.AdditionalEquipment)
                .HasForeignKey<AdditionalEquipment>(d => d.CarId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AirConditioningType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("air_conditioning_types");

            entity.HasIndex(e => e.Name, "name").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("branches");

            entity.HasIndex(e => e.Address, "address").IsUnique();

            entity.HasIndex(e => e.Email, "email").IsUnique();

            entity.HasIndex(e => e.PhoneNumber, "phone_number").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(30)
                .HasColumnName("phone_number");
        });

        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("cars");

            entity.HasIndex(e => e.BranchId, "branch_id");

            entity.HasIndex(e => e.CarCategoryId, "car_category_id");

            entity.HasIndex(e => e.FuelTypeId, "fuel_type_id");

            entity.HasIndex(e => e.LicensePlate, "license_plate").IsUnique();

            entity.HasIndex(e => e.TransmissionId, "transmission_id");

            entity.HasIndex(e => e.WheelDriveTypeId, "wheel_drive_type_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Availability).HasColumnName("availability");
            entity.Property(e => e.BatteryCapacity)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("int(11)")
                .HasColumnName("battery_capacity");
            entity.Property(e => e.BranchId)
                .HasColumnType("int(11)")
                .HasColumnName("branch_id");
            entity.Property(e => e.Brand)
                .HasMaxLength(50)
                .HasColumnName("brand");
            entity.Property(e => e.CarCategoryId)
                .HasColumnType("int(11)")
                .HasColumnName("car_category_id");
            entity.Property(e => e.Color)
                .HasMaxLength(50)
                .HasColumnName("color");
            entity.Property(e => e.CubicCapacity)
                .HasColumnType("int(11)")
                .HasColumnName("cubic_capacity");
            entity.Property(e => e.DefaultPricePerDay)
                .HasColumnType("int(11)")
                .HasColumnName("default_price_per_day");
            entity.Property(e => e.FuelTypeId)
                .HasColumnType("int(11)")
                .HasColumnName("fuel_type_id");
            entity.Property(e => e.ImgUrl)
                    .HasMaxLength(500)
                .HasColumnName("img_url");
            entity.Property(e => e.InspectionExpiryDate)
                .HasColumnType("date")
                .HasColumnName("inspection_expiry_date");
            entity.Property(e => e.LastServiceDate)
                .HasColumnType("date")
                .HasColumnName("last_service_date");
            entity.Property(e => e.LicensePlate)
                .HasMaxLength(20)
                .HasColumnName("license_plate");
            entity.Property(e => e.LuggageCapacity)
                .HasColumnType("int(11)")
                .HasColumnName("luggage_capacity");
            entity.Property(e => e.Mileage)
                .HasColumnType("int(11)")
                .HasColumnName("mileage");
            entity.Property(e => e.Model)
                .HasMaxLength(50)
                .HasColumnName("model");
            entity.Property(e => e.NumberOfDoors)
                .HasColumnType("smallint(6)")
                .HasColumnName("number_of_doors");
            entity.Property(e => e.NumberOfSeats)
                .HasColumnType("smallint(6)")
                .HasColumnName("number_of_seats");
            entity.Property(e => e.OwnWeight)
                .HasColumnType("int(11)")
                .HasColumnName("own_weight");
            entity.Property(e => e.PerformanceHp)
                .HasColumnType("int(11)")
                .HasColumnName("performance_hp");
            entity.Property(e => e.PerformanceKw)
                .HasColumnType("int(11)")
                .HasColumnName("performance_kw");
            entity.Property(e => e.Price)
                .HasColumnType("int(11)")
                .HasColumnName("price");
            entity.Property(e => e.TankCapacity)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("int(11)")
                .HasColumnName("tank_capacity");
            entity.Property(e => e.TotalWeight)
                .HasColumnType("int(11)")
                .HasColumnName("total_weight");
            entity.Property(e => e.TransmissionId)
                .HasColumnType("int(11)")
                .HasColumnName("transmission_id");
            entity.Property(e => e.WheelDriveTypeId)
                .HasColumnType("int(11)")
                .HasColumnName("wheel_drive_type_id");
            entity.Property(e => e.Year)
                .HasColumnType("year(4)")
                .HasColumnName("year");

            entity.HasOne(d => d.Branch).WithMany(p => p.Cars)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.CarCategory).WithMany(p => p.Cars)
                .HasForeignKey(d => d.CarCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.FuelType).WithMany(p => p.Cars)
                .HasForeignKey(d => d.FuelTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.TransmissionType).WithMany(p => p.Cars)
                .HasForeignKey(d => d.TransmissionId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.WheelDriveType).WithMany(p => p.Cars)
                .HasForeignKey(d => d.WheelDriveTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<CarCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("car_categories");

            entity.HasIndex(e => e.Name, "name").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<FuelType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("fuel_types");

            entity.HasIndex(e => e.Name, "name").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Rental>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("rentals");

            entity.HasIndex(e => e.CarId, "car_id");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.CarId)
                .HasColumnType("int(11)")
                .HasColumnName("car_id");
            entity.Property(e => e.Damage)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("damage");
            entity.Property(e => e.EndDate)
                .HasColumnType("date")
                .HasColumnName("end_date");
            entity.Property(e => e.FullPrice)
                .HasColumnType("int(11)")
                .HasColumnName("full_price");
            entity.Property(e => e.ReturnDate)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("date")
                .HasColumnName("return_date");
            entity.Property(e => e.StartDate)
                .HasColumnType("date")
                .HasColumnName("start_date");
            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("user_id");

            entity.HasOne(d => d.Car).WithMany(p => p.Rentals)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.User).WithMany(p => p.Rentals)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<TransmissionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("transmission_types");

            entity.HasIndex(e => e.Name, "name").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "email").IsUnique();

            entity.HasIndex(e => e.PhoneNumber, "phone_number").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.BirthDate)
                .HasColumnType("date")
                .HasColumnName("birth_date");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(30)
                .HasColumnName("phone_number");
            entity.Property(e => e.Role)
                .HasMaxLength(30)
                .HasColumnName("role");
        });

        modelBuilder.Entity<WheelDriveType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("wheel_drive_types");

            entity.HasIndex(e => e.Name, "name").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
