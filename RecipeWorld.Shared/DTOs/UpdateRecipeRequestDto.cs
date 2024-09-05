using FluentValidation;
using RecipeWorld.Shared.Entities;

namespace RecipeWorld.Shared.DTOs
{
    public class UpdateRecipeRequestDto
    {
        public string Title { get; set; } = null!;

        public List<string>? Ingredients { get; set; }

        public string Instructions { get; set; } = string.Empty;
    }

    public class UpdateRecipeRequestDtoValidator : AbstractValidator<UpdateRecipeRequestDto>
    {
        public UpdateRecipeRequestDtoValidator(IValidator<Recipe> recipeValidator)
        {
            RuleFor(dto => new Recipe
            {
                Title = dto.Title,
                Ingredients = dto.Ingredients,
                Instructions = dto.Instructions
            }).SetValidator(recipeValidator);
        }
    }
}