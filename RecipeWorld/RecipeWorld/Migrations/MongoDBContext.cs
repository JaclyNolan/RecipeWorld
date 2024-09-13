using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RecipeWorld.Constants;
using RecipeWorld.Settings;
using RecipeWorld.Shared.Entities;

public class MongoDBContext
{
    private readonly IMongoDatabase _database;

    public MongoDBContext(IOptions<MongoDBSettings> settings, IMongoClient client)
    {
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<Recipe> GetRecipeCollection()
    {
        return _database.GetCollection<Recipe>(MongoDbCollectionNames.Recipe);
    }

    public IMongoCollection<ApplicationUser> GetUserCollection()
    {
        return _database.GetCollection<ApplicationUser>(MongoDbCollectionNames.ApplicationUser);
    }

    public IMongoCollection<ApplicationRole> GetRoleCollection()
    {
        return _database.GetCollection<ApplicationRole>(MongoDbCollectionNames.ApplicationRole);
    }
}
