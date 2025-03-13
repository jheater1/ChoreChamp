using ChoreChamp.API.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChoreChamp.API.Infrastructure.Persistence;

public class ChoreChampDbContext : DbContext
{
    public ChoreChampDbContext(DbContextOptions<ChoreChampDbContext> options)
        : base(options) { }

    public ChoreChampDbContext() { }

    public virtual DbSet<Chore> Chores { get; set; }
    public virtual DbSet<User> Users { get; set; }
}
