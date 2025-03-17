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
    public DbSet<Reward> Rewards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.OwnsOne(
                u => u.Password,
                p =>
                {
                    p.Property(p => p.PasswordHash).HasColumnName("PasswordHash").IsRequired();
                }
            );

            e.OwnsOne(
                u => u.Points,
                p =>
                {
                    p.Property(p => p.Value).HasColumnName("Points").IsRequired();
                }
            );
        });
    }
}
