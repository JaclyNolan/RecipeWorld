using Blazorise;
using Blazorise.Icons.Material;
using Blazorise.Material;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using MongoDB.Bson;
using MongoDB.Driver;
using RecipeWorld.Components;
using RecipeWorld.Constants;
using RecipeWorld.Hubs;
using RecipeWorld.Migrations;
using RecipeWorld.Services;
using RecipeWorld.Settings;
using RecipeWorld.Shared.DTOs;
using RecipeWorld.Shared.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddMaterialProviders()
    .AddMaterialIcons();
//builder.Services.AddControllers();

builder.WebHost.UseStaticWebAssets();
// Load MongoDB settings from configuration
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDBSettings"));

builder.Services.AddSignalR();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        ["application/octet-stream"]);
});
builder.Services.AddAutoMapper(typeof(Program).Assembly);

var mongoDbSettings = builder.Configuration.GetSection("MongoDBSettings").Get<MongoDBSettings>();
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddMongoDbStores<ApplicationUser, ApplicationRole, ObjectId>(
        builder.Configuration.GetConnectionString("MongoDB"),
        mongoDbSettings!.DatabaseName
    )
    .AddDefaultTokenProviders();

// Register MongoClient using the connection string from ConnectionStrings
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("MongoDB");
    return new MongoClient(connectionString);
});
builder.Services.AddSingleton<MongoDBContext>();
builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddScoped<IValidator<Recipe>, RecipeValidator>();
builder.Services.AddScoped<IValidator<string>, IngredientValidator>();
builder.Services.AddScoped<IValidator<CreateRecipeRequestDto>, CreateRecipeRequestDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateRecipeRequestDto>, UpdateRecipeRequestDtoValidator>();


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    await app.SeedDataAsync();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseResponseCompression();
app.MapHub<RecipeHub>(RouteNames.Hub.Recipe);

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(RecipeWorld.Client._Imports).Assembly);
//app.MapControllers();

app.Run();
