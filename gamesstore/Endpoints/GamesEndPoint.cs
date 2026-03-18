using System;
using gamesstore.Data;
using gamesstore.Dtos;
using gamesstore.Models;
using Microsoft.EntityFrameworkCore;

namespace gamesstore.Endpoints;

public static class GamesEndPoint
{

    const string GetGameEndpointName = "GetGame";

    public static void MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");


        // GET /games 
        group.MapGet("",async (GameStoreContext dbContext) => await dbContext.Games
                            .Include(game => game.Genre)
                            .Select(game => new GameSummaryDto(
                                game.Id,
                                game.Name,
                                game.Genre!.Name,
                                game.Price,
                                game.ReleaseDate
                            ))
                            .AsNoTracking()
                            .ToListAsync());


        // GET /games/{id}
        group.MapGet("/{id}", async (int id,GameStoreContext dbContext) =>
        {
            var game = await dbContext.Games.FindAsync(id);
            return game is null ? Results.NotFound() : Results.Ok(
                new GameDetailsDto (
                    game.Id,
                    game.Name,
                    game.GenreId,
                    game.Price,
                    game.ReleaseDate
                )
            );
        }).WithName(GetGameEndpointName);


        // POST /games

        group.MapPost("", async (CreateGameDto createGameDto, GameStoreContext dbContext) =>
        {
            Game game = new()
            {
            Name = createGameDto.Name ,
            GenreId = createGameDto.GenreId,
            Price = createGameDto.Price,
            ReleaseDate = createGameDto.ReleaseDate
            };

            // games.Add(newGame);
            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();

            GameDetailsDto gameDto = new(
                game.Id,
                game.Name,
                game.GenreId,
                game.Price,
                game.ReleaseDate
            );
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = gameDto.Id }, gameDto);
        });

        // PUT /games/{id}
        group.MapPut("/{id}", async (int id,UpdateGameDto updateGame, GameStoreContext dbContext) =>
        {
            var index = await dbContext.Games.FindAsync(id);

            if (index is null)
            {
                return Results.NotFound();
            } 
            index.Name = updateGame.Name;
            index.GenreId = updateGame.GenreId;
            index.Price = updateGame.Price;
            index.ReleaseDate = updateGame.ReleaseDate;

            await dbContext.SaveChangesAsync();
      
            return Results.NoContent();
        });



        //DELETE /games/{id}
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            await dbContext.Games.Where(game => game.Id == id).ExecuteDeleteAsync();

            return Results.NoContent();
        }); 
        }
}