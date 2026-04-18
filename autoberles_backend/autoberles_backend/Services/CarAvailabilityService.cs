using autoberles_backend;
using Microsoft.EntityFrameworkCore;

public class CarAvailabilityService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public CarAvailabilityService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<CarRentalContext>();
                var now = DateTime.Now.Date;
                var cars = await context.Cars.Include(c => c.Rentals).ToListAsync(stoppingToken);

                bool changed = false;

                foreach (var car in cars)
                {
                    bool newAvailability = !car.Rentals.Any(r => r.ReturnDate == null && r.StartDate!.Value.Date <= now && now <= r.EndDate!.Value.Date);

                    if (car.Availability != newAvailability)
                    {
                        car.Availability = newAvailability;
                        changed = true;
                    }
                }

                if (changed)
                    await context.SaveChangesAsync(stoppingToken);
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}