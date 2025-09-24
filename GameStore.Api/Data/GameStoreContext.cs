using GameStore.Api.Entities;
using Microsoft.EntityFrameworkCore;
namespace GameStore.Api.Data;

public class GameStoreContext(DbContextOptions<GameStoreContext> options) : DbContext(options)
{
    //Create a games table1
    public DbSet<Game> Games => Set<Game>(); //read only unlike public DbSet<Game> Games { get; set; }
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>().HasData(
            new { Id = 1, Name = "Fighting" },
new { Id = 2, Name = "Adventure" },
new { Id = 3, Name = "Racing" },
new { Id = 4, Name = "Strategy" },
new { Id = 5, Name = "Sports" },
new { Id = 6, Name = "Role-Playing" }

        );
    }

}