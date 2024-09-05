using FluentValidation;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RecipeWorld.Shared.Entities
{
    //When modifying the Recipe model makes sure to use the schema version patten
    public class Recipe : BaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Title { get; set; } = null!;

        public List<string> Ingredients { get; set; } = [];

        public string Instructions { get; set; } = string.Empty;
    }

    public class RecipeValidator : AbstractValidator<Recipe>
    {
        public RecipeValidator(IValidator<string> ingredientValidator)
        {
            RuleFor(recipe => recipe.Title)
                .NotEmpty().WithMessage("The title is required")
                .MaximumLength(250).WithMessage("The title cannot be longer than 250 characters");

            RuleFor(recipe => recipe.Ingredients)
                .NotNull().WithMessage("Ingredients list cannot be null")
                .NotEmpty().WithMessage("Ingredients list cannot be empty")
                .Must(list => list.Count <= 100).WithMessage("The ingredients list cannot contain more than 100 items.");

            RuleForEach(recipe => recipe.Ingredients)
                .SetValidator(ingredientValidator);

            RuleFor(recipe => recipe.Instructions)
                .MaximumLength(5000).WithMessage("The instructions cannot exceed 5000 characters.");
        }
    }

    public class IngredientValidator : AbstractValidator<string>
    {
        public IngredientValidator()
        {
            RuleFor(ingredient => ingredient)
                .NotEmpty().WithMessage("Ingredient cannot be empty.")
                .Length(2, 200).WithMessage("Ingredient must be between 2 and 200 characters.");
        }
    }
}
