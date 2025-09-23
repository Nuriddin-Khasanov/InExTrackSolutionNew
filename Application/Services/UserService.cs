using Application.DTOs;
using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Models;
using Mapster;

namespace Application.Services;

public class UserService(IUserRepository _userRepository, IJWTService _jwtService, IFileService _fileService) : IUserService
{
    public async Task<ApiResponse<AuthResultDto>> AuthenticateAsync(string username, string password)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        if (user == null || !user.IsActive)
            return new ApiResponse<AuthResultDto>(400, "Пользователь не найден или заблокирован!");

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return new ApiResponse<AuthResultDto>(401, "Неверный пароль!");

        var token = _jwtService.GenerateToken(user);

        var _userResponseDto = user.Adapt<UserResponseDto>();

        var result = new AuthResultDto
        {
            UserResponseDto = _userResponseDto,
            Token = token
        };

        return new ApiResponse<AuthResultDto>(200, result, "Успешный вход");
    }

    public async Task<ApiResponse<UserResponseDto>> GetUserById(Guid _userId, CancellationToken cancellationToken)
    {
        if (_userId == Guid.Empty)
            return new ApiResponse<UserResponseDto>(400, "Некорректный идентификатор пользователя.");

        var user = (await _userRepository.GetByIdAsync(_userId, cancellationToken)).Adapt<UserResponseDto>();
        // .ContinueWith(t => t.Result.Adapt<UserResponseDto>(), cancellationToken);

        if (user == null)
            return new ApiResponse<UserResponseDto>(404, "Пользователь не найден!");

        return new ApiResponse<UserResponseDto>(200, user, "Пользователь успешно получен!");
    }

    public async Task<ApiResponse<bool>> RegisterUserAsync(UserRequestsDto _user, CancellationToken cancellationToken)
    {
        if (_user == null)
            return new ApiResponse<bool>(400, "Данные пользователя не предоставлены.");

        if (string.IsNullOrWhiteSpace(_user.UserName))
            return new ApiResponse<bool>(400, "Имя пользователя не может быть пустым.");

        if (await _userRepository.ExistsAsync(_user.UserName, _user.Email, _user.PhoneNumber, cancellationToken))
            return new ApiResponse<bool>(400, "Пользователь уже существует, попробуйте изменить имя, Email или номер телефона!");

        _user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(_user.PasswordHash);

        var user = _user.Adapt<User>();

        if (_user.ImageURL != null)
        {
            var savedFile = await _fileService.SaveAsync(_user.ImageURL);
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
        await _userRepository.AddAsync(user);

        return new ApiResponse<bool>(201, true, "Пользователь успешно добавлен!");
    }

    public async Task<ApiResponse<UserResponseDto>> UpdateUserById(Guid _userId, UserRequestsDto userRequestsDto, CancellationToken cancellationToken)
    {
        if (_userId == Guid.Empty)
            return new ApiResponse<UserResponseDto>(400, "Некорректный идентификатор пользователя.");

        var updatedUser = (await _userRepository.UpdateAsync(_userId, userRequestsDto, cancellationToken)).Adapt<UserResponseDto>();

        //if (updatedUser == null)
        //    throw new InvalidOperationException("Пользователь не найден или не обновлен.");

        return new ApiResponse<UserResponseDto>(200, updatedUser, "Пользователь успешно обновлен!");
    }

    public async Task<ApiResponse<bool>> DeleteUser(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            return new ApiResponse<bool>(400, "Некорректный идентификатор пользователя.");

        if (await _userRepository.DeleteAsync(id, cancellationToken))
            return new ApiResponse<bool>(204, true, "Пользователь успешно удален.");
        
        return new ApiResponse<bool>(400, "Пользователь не найден или заблокирован.");
    }


    //public async Task<bool> RegisterUserAsync(string username, string password)
    //{
    //    if (await _userRepository.ExistsAsync(username))
    //        return false;

    //    var user = new User
    //    {
    //        UserName = username,
    //        PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
    //    };

    //    await _userRepository.AddAsync(user);
    //    return true;
    //}

}
