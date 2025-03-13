using ChoreChamp.API.Domain;
using ChoreChamp.API.Infrastructure.Persistence;
using ChoreChamp.API.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;

namespace ChoreChamp.API.Infrastructure.Seeder;

public class DevDataSeeder
{
    private readonly ChoreChampDbContext _dbContext;
    private readonly IPasswordService _passwordService;
    private readonly ILogger<DevDataSeeder> _logger;

    public DevDataSeeder(ChoreChampDbContext dbContext, IPasswordService passwordService, ILogger<DevDataSeeder> logger)
    {
        _dbContext = dbContext;
        _passwordService = passwordService;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            _logger.LogInformation("Starting database seeding...");

            await SeedUsersAsync();
            await SeedChoresAsync();

            await transaction.CommitAsync();
            _logger.LogInformation("Database seeding completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            await transaction.RollbackAsync();
            throw;
        }
    }

    private async Task SeedUsersAsync()
    {
        if (await _dbContext.Users.AsNoTracking().AnyAsync()) return;

        var users = new List<User>
        {
            new User
            {
                Name = "Admin User",
                Email = "admin@example.com",
                PasswordHash = _passwordService.HashPassword("AdminPassword123"),
                IsAdmin = true,
                Points = 0,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                Name = "Regular User",
                Email = "user@example.com",
                PasswordHash = _passwordService.HashPassword("UserPassword123"),
                IsAdmin = false,
                Points = 0,
                CreatedAt = DateTime.UtcNow
            }
        };

        _dbContext.Users.AddRange(users);
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("Seeded users.");
    }

    private async Task SeedChoresAsync()
    {
        if (await _dbContext.Chores.AsNoTracking().AnyAsync()) return;

        var chores = new List<Chore>
        {
            new Chore
            {
                Name = "Take out the trash",
                Description = "Take out the trash from the kitchen and living room",
                Points = 5,
            },
            new Chore
            {
                Name = "Do the dishes",
                Description = "Wash the dishes in the sink and put them away",
                Points = 10,
            }
        };

        _dbContext.Chores.AddRange(chores);
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("Seeded chores.");
    }
}
