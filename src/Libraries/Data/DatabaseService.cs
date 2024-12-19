using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace Libraries.Data;

// ReSharper disable once ClassNeverInstantiated.Global
public class DatabaseService {
    ApplicationDbContext? dbContext;

    // ReSharper disable once ConvertToPrimaryConstructor
    // public DatabaseService()
    //     => ;

    public async Task intialize() {
        try {
            Env.TraversePath().Load();

            const string msg = "Failed to get the environmental value!";
            var connectionString = Env.GetString("DIRECT_URL", msg);

            var options = new DbContextOptionsBuilder<ApplicationDbContext>();
            options.UseNpgsql(connectionString);

            dbContext = new ApplicationDbContext(options.Options);
            var ensure = await dbContext.Database.EnsureCreatedAsync();
            if (!ensure) throw new Exception("Failed to connect to the database!");
        } catch (Exception ex) {
            throw new Exception("Client initialization has failed!", ex);
        }
    }

    public ApplicationDbContext getDbContext()
        => dbContext ?? throw new Exception(
            "Database context is not initialized. Call initialize() first.");
}
