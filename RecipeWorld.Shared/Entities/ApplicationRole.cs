using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson;
using MongoDbGenericRepository.Attributes;
using RecipeWorld.Constants;

namespace RecipeWorld.Shared.Entities
{
    [CollectionName(MongoDbCollectionNames.ApplicationRole)]
    public class ApplicationRole : MongoIdentityRole<ObjectId>
    {
    }
}
