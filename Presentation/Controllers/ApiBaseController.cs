using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
public abstract class ApiBaseController : ControllerBase
{
    protected Guid? UserUid => User?.Identity is { IsAuthenticated: true }
        ? Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value)
        : null;

    protected Guid GetUserId()
    {
        return UserUid is not null ? UserUid.Value : throw new InvalidOperationException("UserUid is null.");
    }
}
