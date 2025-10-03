using Application.DTOs;
using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Models;
using Mapster;

namespace Application.Services;

public class UserService(IUserRepository userRepository, IJWTService jwtService, IFileService fileService) : IUserService
{
    public async Task<ApiResponse<AuthResultDto>> AuthenticateAsync(string username, string password)
    {
        var user = await userRepository.GetByUsernameAsync(username);
        if (user == null || !user.IsActive) 
            throw new NotFoundException("Пользователь не найден или заблокирован.");
        // return new ApiResponse<AuthResultDto>(400, "Пользователь не найден или заблокирован!");

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return new ApiResponse<AuthResultDto>(401, "Неверный пароль!");

        var token = jwtService.GenerateToken(user);

        var userResponseDto = user.Adapt<UserResponseDto>();

        var result = new AuthResultDto
        {
            UserResponseDto = userResponseDto,
            Token = token
        };

        return new ApiResponse<AuthResultDto>(200, result, "Успешный вход");
    }

    public async Task<ApiResponse<UserResponseDto>> GetUserById(Guid userId, CancellationToken cancellationToken)
    {
        if (userId == Guid.Empty)
            return new ApiResponse<UserResponseDto>(400, "Некорректный идентификатор пользователя.");

        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        var userDto = user.Adapt<UserResponseDto>();
        // .ContinueWith(t => t.Result.Adapt<UserResponseDto>(), cancellationToken);

        if (user == null)
            return new ApiResponse<UserResponseDto>(404, "Пользователь не найден!");

        return new ApiResponse<UserResponseDto>(200, userDto, "Пользователь успешно получен!");
    }

    public async Task<ApiResponse<bool>> RegisterUserAsync(UserRequestsDto? _user, CancellationToken cancellationToken)
    {
        if (_user == null)
            return new ApiResponse<bool>(400, "Данные пользователя не предоставлены.");

        if (string.IsNullOrWhiteSpace(_user.UserName))
            return new ApiResponse<bool>(400, "Имя пользователя не может быть пустым.");

        if (await userRepository.ExistsAsync(_user.UserName, _user.Email, _user.PhoneNumber, cancellationToken))
            return new ApiResponse<bool>(400, "Пользователь уже существует, попробуйте изменить имя, Email или номер телефона!");

        _user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(_user.PasswordHash);

        var user = _user.Adapt<User>();

        if (_user.ImageURL != null)
        {
            var savedFile = await fileService.SaveAsync(_user.ImageURL);
            if(savedFile == null)
                return new ApiResponse<bool>(400, "Ошибка при сохранении файла изображения пользователя.");
            user.Image = new UserFile()
            {
                UserId = user.Id,
                Name = savedFile.Name,
                Url = savedFile.Url,
                Size = savedFile.Size,
                Extension = savedFile.Extension
            };
        }

        // Сохраняем пользователя в БД
        await userRepository.AddAsync(user);

        return new ApiResponse<bool>(201, true, "Пользователь успешно добавлен!");
    }

    public async Task<ApiResponse<UserResponseDto>> UpdateUserById(Guid userId, UserRequestsDto? userRequestsDto, CancellationToken cancellationToken)
    {
        try
        {
            if (userRequestsDto == null)
                return new ApiResponse<UserResponseDto>(400, "Данные пользователя не предоставлены.");

            if (string.IsNullOrWhiteSpace(userRequestsDto.UserName))
                return new ApiResponse<UserResponseDto>(400, "Имя пользователя не может быть пустым.");

            userRequestsDto.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userRequestsDto.PasswordHash);

            var user = userRequestsDto.Adapt<User>();
            user.Id = userId;

            var userPicture = await userRepository.GetUserPicture(userId, cancellationToken);
            if (!string.IsNullOrEmpty(userPicture))
                await fileService.RemoveAsync(userPicture);

            if (userRequestsDto.ImageURL != null)
            {
                var savedFile = await fileService.SaveAsync(userRequestsDto.ImageURL);
                if (savedFile == null)
                    return new ApiResponse<UserResponseDto>(400, "Ошибка при сохранении файла изображения пользователя.");
                user.Image = new UserFile()
                {
                    UserId = user.Id,
                    Name = savedFile.Name,
                    Url = savedFile.Url,
                    Size = savedFile.Size,
                    Extension = savedFile.Extension
                };
            }

            var updatedUser = (await userRepository.UpdateAsync(userId, user, cancellationToken)).Adapt<UserResponseDto>();
            return new ApiResponse<UserResponseDto>(200, updatedUser, "Пользователь успешно обновлен!");
        }
        catch (Exception ex)
        {
            return new ApiResponse<UserResponseDto>(500, $"Произошла ошибка при обновлении пользователя: {ex.Message}");
        }
    }

    public async Task<ApiResponse<bool>> DeleteUser(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            return new ApiResponse<bool>(400, "Некорректный идентификатор пользователя.");

        if (await userRepository.DeleteAsync(id, cancellationToken))
            return new ApiResponse<bool>(204, true, "Пользователь успешно удален.");
        
        return new ApiResponse<bool>(400, "Пользователь не найден или заблокирован.");
    }

}
