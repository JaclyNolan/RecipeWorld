using Microsoft.AspNetCore.Mvc;
using RecipeWorld.Exceptions;
using RecipeWorld.Services;
using RecipeWorld.Shared.DTOs;

namespace RecipeWorld.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _recipeService;

        public RecipeController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        // GET: api/recipe
        [HttpGet]
        public async Task<IActionResult> GetAllRecipes()
        {
            var recipes = await _recipeService.GetAllRecipesAsync();
            return Ok(recipes);
        }

        // GET: api/recipe/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecipeById(string id)
        {
            var recipe = await _recipeService.GetRecipeByIdAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }
            return Ok(recipe);
        }

        // POST: api/recipe
        [HttpPost]
        public async Task<IActionResult> CreateRecipe([FromBody] CreateRecipeRequestDto request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            var newRecipe = await _recipeService.CreateRecipeAsync(request);
            return CreatedAtAction(nameof(GetRecipeById), new { id = newRecipe.Id }, newRecipe);
        }

        // PUT: api/recipe/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecipe(string id, [FromBody] UpdateRecipeRequestDto request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            var existingRecipe = await _recipeService.GetRecipeByIdAsync(id);
            if (existingRecipe == null)
            {
                return NotFound();
            }

            await _recipeService.UpdateRecipeAsync(id, request);
            return NoContent();
        }

        // DELETE: api/recipe/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(string id)
        {
            try
            {
                await _recipeService.DeleteRecipeAsync(id);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex);
            }
            return NoContent();
        }
    }
}
