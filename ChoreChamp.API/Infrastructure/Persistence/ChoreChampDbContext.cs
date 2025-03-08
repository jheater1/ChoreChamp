using ChoreChamp.API.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChoreChamp.API.Infrastructure.Persistence;

public class ChoreChampDbContext : DbContext
{
    public DbSet<Chore> Chores { get; set; }
}
