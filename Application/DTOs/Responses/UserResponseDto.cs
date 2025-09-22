﻿using Microsoft.AspNetCore.Http;

namespace InExTrack.Application.DTOs.Responses
{
    public class UserResponseDto
    {
        public required string UserName { get; set; }

        public required string FullName { get; set; }

        public required string Email { get; set; }

        public required string PhoneNumber { get; set; }

        public IFormFile? ImageURL { get; set; }
    }
}
