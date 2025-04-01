using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using MusicAndReaction.DataServer.Database;
using Microsoft.Extensions.Configuration;


namespace MusicAndReaction.DataServer.Functions;
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlite(config.GetConnectionString("SQLiteConnection"));

        return new AppDbContext(optionsBuilder.Options, config);
    }