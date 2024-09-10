using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;
using RecipeWorld.Exceptions;
using RecipeWorld.Hubs;
using RecipeWorld.Shared.DTOs;
using RecipeWorld.Shared.Entities;

namespace RecipeWorld.Services
{
    public interface IRecipeService
    {
        Task<IEnumerable<GetRecipeResponseDto>> GetAllRecipesAsync();
        Task<GetRecipeResponseDto?> GetRecipeByIdAsync(string id);
        Task<GetRecipeResponseDto> CreateRecipeAsync(CreateRecipeRequestDto createRecipeRequest);
        Task UpdateRecipeAsync(string id, UpdateRecipeRequestDto updateRecipeRequest);
        Task DeleteRecipeAsync(string id);
    }

    public class RecipeService : IRecipeService
    {
        private readonly IMapper _mapper;
        private readonly MongoDBContext _context;
        private readonly IMongoCollection<Recipe> _recipeCollection;
        private readonly IHubContext<RecipeHub> _hubContext;

        public RecipeService(MongoDBContext context, IMapper mapper, IHubContext<RecipeHub> hubContext)
        {
            _mapper = mapper;
            _context = context;
            _recipeCollection = _context.GetRecipeCollection();
            _hubContext = hubContext;
        }

        public async Task<IEnumerable<GetRecipeResponseDto>> GetAllRecipesAsync()
        {
            return _mapper.Map<List<GetRecipeResponseDto>>(await _recipeCollection.Find(recipe => recipe.DeletedAt == null).ToListAsync());
        }

        public async Task<GetRecipeResponseDto?> GetRecipeByIdAsync(string id)
        {
            try
            {
                return _mapper.Map<GetRecipeResponseDto?>(await _recipeCollection.Find(recipe => recipe.Id == id & recipe.DeletedAt == null).FirstOrDefaultAsync());
            }
            catch (FormatException)
            {
            }
            return null;
        }

        public async Task<GetRecipeResponseDto> CreateRecipeAsync(CreateRecipeRequestDto createRecipeRequest)
        {
            var recipe = _mapper.Map<Recipe>(createRecipeRequest);
            await _recipeCollection.InsertOneAsync(recipe);
            await _hubContext.Clients.All.SendAsync("ReceiveRecipeUpdate");
            return _mapper.Map<GetRecipeResponseDto>(recipe);
        }

        public async Task UpdateRecipeAsync(string id, UpdateRecipeRequestDto updateRecipeRequest)
        {
            var newRecipe = _mapper.Map<Recipe>(updateRecipeRequest);
            newRecipe.UpdatedAt = DateTime.UtcNow;
            newRecipe.Id = id;
            await _recipeCollection.ReplaceOneAsync(recipe => recipe.Id == id, newRecipe);
            await _hubContext.Clients.All.SendAsync("ReceiveRecipeUpdate");
        }

        public async Task DeleteRecipeAsync(string id)
        {
            var recipe = await _recipeCollection.Find(recipe => recipe.Id == id).FirstOrDefaultAsync()
                ?? throw new NotFoundException("Recipe doesn't exist");
            recipe.DeletedAt = DateTime.UtcNow;
            await _recipeCollection.ReplaceOneAsync(recipe => recipe.Id == id, recipe);
            await _hubContext.Clients.All.SendAsync("ReceiveRecipeUpdate");
        }
    }
}
