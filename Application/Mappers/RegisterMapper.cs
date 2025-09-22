using InExTrack.Application.DTOs;
using InExTrack.Application.DTOs.Requests;
using InExTrack.Application.DTOs.Responses;
using InExTrack.Domain.Models;
using Mapster;

namespace InExTrack.Application.Mappers
{
    public class RegisterMapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<User, UserRequestsDto>();
            config.NewConfig<UserRequestsDto, User>();

            config.NewConfig<User, UserResponseDto>();
            config.NewConfig<UserResponseDto, User>();

            config.NewConfig<Category, CategoryDto>();
            config.NewConfig<CategoryDto, Category>();

            config.NewConfig<UserCategory, UserCategoryDto>();
            config.NewConfig<UserCategoryDto, UserCategory>();
        }
    }
}
