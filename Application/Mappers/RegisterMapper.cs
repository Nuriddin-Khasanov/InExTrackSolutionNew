using Application.DTOs;
using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Domain.Models;
using Mapster;

namespace Application.Mappers;

public class RegisterMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, UserRequestsDto>();
        config.NewConfig<UserRequestsDto, User>();

            config.NewConfig<User, UserResponseDto>()
                .Map(dest => dest.ImageURL, 
                    src => src.Image.Url);
            config.NewConfig<UserResponseDto, User>();

        config.NewConfig<Category, CategoryDto>();
        config.NewConfig<CategoryDto, Category>();

        config.NewConfig<UserCategory, UserCategoryDto>();
        config.NewConfig<UserCategoryDto, UserCategory>();
    }
}
