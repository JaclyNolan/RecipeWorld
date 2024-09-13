using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson;
using MongoDbGenericRepository.Attributes;
using RecipeWorld.Constants;

namespace RecipeWorld.Shared.Entities
{
    [CollectionName(MongoDbCollectionNames.ApplicationUser)]
    public class ApplicationUser : MongoIdentityUser<ObjectId>
    {
    }
}
