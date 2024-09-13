using Microsoft.AspNetCore.Identity;
using RecipeWorld.Constants;
using RecipeWorld.Shared.Entities;

namespace RecipeWorld.Migrations
{
    public static class ApplicationSeeder
    {
        public static async Task SeedDataAsync(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                Console.WriteLine("Development: Started Seeding Data");
                var context = scope.ServiceProvider.GetRequiredService<MongoDBContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

                await SeedRoles(roleManager);
                await SeedUsers(context, userManager);
            }
        }

        private static async Task SeedUsers(MongoDBContext context, UserManager<ApplicationUser> userManager)
        {
            var userCollection = context.GetUserCollection();

            // Check if users already exist to prevent duplicate seeding
            if (await userCollection.EstimatedDocumentCountAsync() == 0)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@recipeworld.com",
                };

                var user1 = new ApplicationUser
                {
                    UserName = "user1",
                    Email = "user1@recipeworld.com",
                };

                var adminResult = await userManager.CreateAsync(adminUser, "AdminPassword123!");
                if (adminResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, RoleNames.Admin);
                    Console.WriteLine("Admin user seeded successfully.");
                }
                else
                {
                    Console.WriteLine("Error seeding admin user: " + string.Join(", ", adminResult.Errors.Select(e => e.Description)));
                }

                var userResult = await userManager.CreateAsync(user1, "UserPassword123!");
                if (userResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(user1, RoleNames.User);
                    Console.WriteLine("User1 seeded successfully.");
                }
                else
                {
                    Console.WriteLine("Error seeding user1: " + string.Join(", ", userResult.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                Console.WriteLine("Users Collection has already been seeded");
            }
        }

        private static async Task SeedRoles(RoleManager<ApplicationRole> roleManager)
        {
            var roles = new List<string> { RoleNames.Admin, RoleNames.User };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var newRole = new ApplicationRole { Name = role };
                    var result = await roleManager.CreateAsync(newRole);

                    if (result.Succeeded)
                    {
                        Console.WriteLine($"Role '{role}' seeded successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Error seeding role '{role}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
                else
                {
                    Console.WriteLine($"Role '{role}' already exists.");
                }
            }
        }
    }
}
