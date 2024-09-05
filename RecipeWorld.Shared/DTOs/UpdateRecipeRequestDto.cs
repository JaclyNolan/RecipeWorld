namespace RecipeWorld.Shared.DTOs
{
    public class UpdateRecipeRequestDto
    {
        public string Title { get; set; } = null!;

        public List<string>? Ingredients { get; set; }

        public string Instructions { get; set; } = string.Empty;
    }
}