using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RecipeWorld.Shared.Entities
{
    //When modifing the Recipe model makes sure to use the schema version patten
    public class Recipe : BaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Title { get; set; } = null!;
        public List<string>? Ingredients { get; set; }
        public string Instructions { get; set; } = string.Empty;
    }
}
