using ChoreChamp.API.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChoreChamp.API.Infrastructure.Persistence;

public interface IChoreChampDbContext
{
    DbSet<Chore> Chores { get; set; }
    DbSet<User> Users { get; set; }
    DbSet<AssignedChore> AssignedChores { get; set; }
    DbSet<Reward> Rewards { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
