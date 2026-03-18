
using gamesstore.Data;
using gamesstore.Dtos;
using Microsoft.EntityFrameworkCore;

namespace gamesstore.Endpoints;

public static class GenresEndPoints
{
    public static void MapGenresEndPoints(this WebApplication app)
    {
        var group = app.MapGroup("/genres");

        // GET /genres
        group.MapGet("/", async (GameStoreContext dbContext) =>
        await dbContext.Genres
        .Select(genre => new GenreDto(genre.Id, genre.Name))
        .AsNoTracking()
        .ToListAsync());
    }
}