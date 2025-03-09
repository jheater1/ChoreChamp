using ChoreChamp.API.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChoreChamp.API.Infrastructure.Persistence;

public class ChoreChampDbContext(DbContextOptions<ChoreChampDbContext> options) : DbContext(options)
{
    public DbSet<Chore> Chores { get; set; }
}
