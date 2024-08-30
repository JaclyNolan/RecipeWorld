namespace RecipeWorld.Shared.DTOs
{
    public class CreateRecipeRequestDto
    {
        public List<string>? Ingredients { get; set; }
        public string Instructions { get; set; } = string.Empty;
    }
}