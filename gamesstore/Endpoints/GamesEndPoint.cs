using System;
using gamesstore.Data;
using gamesstore.Dtos;
using gamesstore.Models;

namespace gamesstore.Endpoints;

public static class GamesEndPoint
{

    const string GetGameEndpointName = "GetGame";

    private static readonly List<GameDto> games = [
    new (
        1,
        "Street Fighter II", 
        "Fighting", 19.99m, new DateOnly(1991, 7, 1)),
    new (
        2,
        "Super Mario Bros.",
        "Platformer",
        29.99m,
        new DateOnly(1985, 9, 13)
    ),
    new (
        3,
        "The Legend of Zelda: Ocarina of Time",
        "Action-Adventure",
        39.99m,
        new DateOnly(1998, 11, 21)
    ),
    new (
        4,
        "Minecraft",
        "Sandbox",
        26.95m,
        new DateOnly(2011, 11, 18)
    ),
    ];


    public static void MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");


        // GET /games 
        group.MapGet("", () => games);


        // GET /games/{id}
        group.MapGet("/{id}", (int id) =>
        {
            var game =games.Find(game => game.Id == id);
            return game is null ? Results.NotFound() : Results.Ok(game);
        }).WithName(GetGameEndpointName);


        // POST /games

        group.MapPost("", (CreateGameDto createGameDto, GameStoreContext dbContext) =>
        {
           
            // var newGame = new GameDto(
            //     games.Count + 1,
            //     createGameDto.Name,
            //     createGameDto.Genre,
            //     createGameDto.Price,
            //     createGameDto.ReleaseDate
            // );

            Game game = new()
            {
               Name = createGameDto.Name ,
               GenreId = createGameDto.GenreId,
               Price = createGameDto.Price,
               ReleaseDate = createGameDto.ReleaseDate
            };

            // games.Add(newGame);
            dbContext.Games.Add(game);
            dbContext.SaveChanges();

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
        group.MapPut("/{id}", (int id,UpdateGameDto updateGame) =>
        {
            var index = games.FindIndex(game => game.Id == id);
            if (index == -1)
            {
                return Results.NotFound();
            }   
            games[index] = new GameDto(
            id,
            updateGame.Name,
            updateGame.Genre,
            updateGame.Price,
            updateGame.ReleaseDate
        );
            return Results.NoContent();
        });



        //DELETE /games/{id}
        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);
            return Results.NoContent();
        }); 
        }
}