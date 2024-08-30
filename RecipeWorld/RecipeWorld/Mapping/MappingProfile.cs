using AutoMapper;
using RecipeWorld.Shared.DTOs;
using RecipeWorld.Shared.Entities;

namespace RecipeWorld.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateRecipeRequestDto, Recipe>();
            CreateMap<UpdateRecipeRequestDto, Recipe>();
            CreateMap<Recipe, GetRecipeResponseDto>();
        }
    }
}