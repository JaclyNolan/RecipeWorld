namespace RecipeWorld.Shared.DTOs
{
    public class GetRecipeResponseDto
    {
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        public List<string>? Ingredients { get; set; }
        public string Instructions { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}