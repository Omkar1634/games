
using gamesstore.Models;
using Microsoft.EntityFrameworkCore;

namespace gamesstore.Data;

public static class DataExtensions
{
    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<GameStoreContext>();
        context.Database.Migrate();
    }


    public static void AddGameStoreDb (this WebApplicationBuilder builder)
    {
        var ConnString = builder.Configuration.GetConnectionString("GameStore");
        builder.Services.AddSqlite<GameStoreContext>(
            ConnString,
            optionsAction: options => options.UseSeeding((context,_) =>
            {
                if (!context.Set<Genre>().Any())
                {
                    context.Set<Genre>().AddRange(
                        new Genre { Name = "Action" },
                        new Genre { Name = "Adventure" },
                        new Genre { Name = "RPG" },
                        new Genre { Name = "Strategy" },
                        new Genre { Name = "Sports" }
                    );
                        context.SaveChanges();
                }
            })
            
            );
    }
}
