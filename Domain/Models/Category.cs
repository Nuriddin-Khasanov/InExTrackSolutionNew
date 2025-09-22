﻿using InExTrack.Domain.Commons;
using InExTrack.Domain.Enums;

namespace InExTrack.Domain.Models;

public class Category : Entity
{
    public string? Name { get; set; }
    public CategoryTypeEnum Type { get; set; } // income or expense
    public string? Description { get; set; }
    public CategoryFile? Image { get; set; }
    public required List<UserCategory> UserCategories { get; set; }
}
