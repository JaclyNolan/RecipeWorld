using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RecipeWorld.Constants;
using RecipeWorld.Shared.Entities;
using System.Security.Claims;

namespace Microsoft.AspNetCore.Routing
{
    internal static class IdentityComponentsEndpointRouteBuilderExtensions
    {
        // These endpoints are required by the Identity Razor components defined in the /Components/Account/Pages directory of this project.
        public static IEndpointConventionBuilder MapAdditionalIdentityEndpoints(this IEndpointRouteBuilder endpoints)
        {
            ArgumentNullException.ThrowIfNull(endpoints);

            // Map Logout endpoint without any prefix
            var logoutEndpoint = endpoints.MapPost(RouteNames.Logout, async (
                ClaimsPrincipal user,
                SignInManager<ApplicationUser> signInManager,
                [FromForm] string returnUrl) =>
            {
                if (string.IsNullOrEmpty(returnUrl))
                {
                    returnUrl = "/";
                }

                await signInManager.SignOutAsync();

                return TypedResults.LocalRedirect($"~/{returnUrl}");
            });

            return logoutEndpoint;
        }
    }
}
