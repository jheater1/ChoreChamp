using ChoreChamp.API.Domain;
using ChoreChamp.API.Infrastructure.Persistence;
using ChoreChamp.API.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;

namespace ChoreChamp.API.Infrastructure.Persistence.Seeder;

public class DevDataSeeder
{
    private readonly ChoreChampDbContext _dbContext;
    private readonly IPasswordService _passwordService;
    private readonly ILogger<DevDataSeeder> _logger;

    public DevDataSeeder(
        ChoreChampDbContext dbContext,
        IPasswordService passwordService,
        ILogger<DevDataSeeder> logger
    )
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
        if (await _dbContext.Users.AsNoTracking().AnyAsync())
            return;

        var users = new List<User>
        {
            new User("Admin User", "admin@example.com", "AdminPassword123", true, _passwordService),
            new User("Regular User", "user@example.com", "UserPassword123", false, _passwordService),
        };

        _dbContext.Users.AddRange(users);
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("Seeded users.");
    }

    private async Task SeedChoresAsync()
    {
        if (await _dbContext.Chores.AsNoTracking().AnyAsync())
            return;

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
            },
        };

        _dbContext.Chores.AddRange(chores);
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("Seeded chores.");
    }
}
