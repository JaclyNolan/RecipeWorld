using RecipeWorld.Components.Pages.RecipePages;

namespace RecipeWorld.Constants
{
    public static class RouteNames
    {
        public const string Login = "/login";
        public const string Index = "/";
        public const string Unauthorized = "/unauthorized";
        public const string Forbidden = "/forbidden";
        public const string NotFound = "/404";
        public const string ServerError = "/server-error";

        public static class Hub
        {
            public const string Recipe = "/hubs/recipe";
        }

        public static class Recipe
        {
            public const string List = "/recipes";
            public const string Create = "/recipes/create";
            public const string Edit = "/recipes/edit/{" + nameof(RecipeEdit.recipeId) + "}";
            public static string GetEditRoute(string? recipeId) => !string.IsNullOrEmpty(recipeId) ? $"/recipes/edit/{recipeId}" : "/recipes/edit";
        }
    }

}
