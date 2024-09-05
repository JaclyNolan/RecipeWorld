using FluentValidation;
using RecipeWorld.Shared.Entities;

namespace RecipeWorld.Shared.DTOs
{
    public class CreateRecipeRequestDto
    {
        public string Title { get; set; } = null!;

        public List<string> Ingredients { get; set; } = [];

        public string Instructions { get; set; } = string.Empty;
    }

    public class CreateRecipeRequestDtoValidator : AbstractValidator<CreateRecipeRequestDto>
    {
        public CreateRecipeRequestDtoValidator(IValidator<Recipe> recipeValidator)
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