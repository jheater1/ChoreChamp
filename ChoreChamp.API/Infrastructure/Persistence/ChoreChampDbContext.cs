using ChoreChamp.API.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChoreChamp.API.Infrastructure.Persistence;

public class ChoreChampDbContext : DbContext, IChoreChampDbContext
{
    public ChoreChampDbContext(DbContextOptions<ChoreChampDbContext> options)
        : base(options) { }

    public DbSet<Chore> Chores { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<AssignedChore> AssignedChores { get; set; }
}
