using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Data;
using Microsoft.EntityFrameworkCore;
using GameStore.Api.Mapping;

namespace GameStore.Api.Endpoints;

public static class GameEndpoints
{
    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games");

        // GET all games
        group.MapGet("/", (GameStoreContext dbContext) =>
            dbContext.Games
                .Include(g => g.Genre) // eager-load Genre
                .Select(g => new GameDto(
                    g.Id,
                    g.Name,
                    g.Genre!.Name, // navigation property
                    g.Price,
                    g.ReleaseDate
                ))
                .ToList()
        );
        // GET game by id
        group.MapGet("/{id}", (int id, GameStoreContext dbContext) =>
        {
            var game = dbContext.Games
                .Include(g => g.Genre)
                .FirstOrDefault(g => g.Id == id);

            return game is not null
                ? Results.Ok(new GameDto(
                    game.Id,
                    game.Name,
                    game.Genre!.Name,
                    game.Price,
                    game.ReleaseDate
                ))
                : Results.NotFound();
        }).WithName("GetGame");

        // POST new game
        group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            Game game = newGame.ToEntity();
            game.Genre = dbContext.Genres.Find(newGame.GenreId);
            // Game game = new()
            // {
            //     Name = newGame.Name,
            //     GenreId = newGame.GenreId,
            //     Price = newGame.Price,
            //     ReleaseDate = newGame.ReleaseDate
            // };

            dbContext.Games.Add(game);
            dbContext.SaveChanges();

            return Results.CreatedAtRoute("GetGame", new { id = game.Id }, game.ToDto());
        });

        // PUT update game
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
        {
            var existingGame = dbContext.Games.Find(id);
            if (existingGame is null)
            {
                return Results.NotFound();
            }

            existingGame.Name = updatedGame.Name;
            existingGame.GenreId = updatedGame.GenreId;
            existingGame.Price = updatedGame.Price;
            existingGame.ReleaseDate = updatedGame.ReleaseDate;

            dbContext.SaveChanges();

            return Results.NoContent();
        });

        // DELETE game
        group.MapDelete("/{id}", (int id, GameStoreContext dbContext) =>
        {
            var game = dbContext.Games.Find(id);
            if (game is null)
            {
                return Results.NotFound();
            }

            dbContext.Games.Remove(game);
            dbContext.SaveChanges();

            return Results.NoContent();
        });

        return group;
    }
}
