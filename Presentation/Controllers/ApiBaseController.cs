using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
public abstract class ApiBaseController : ControllerBase
{
    protected Guid? UserUid => User?.Identity != null && User.Identity.IsAuthenticated
        ? Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value)
        : null;

    protected Guid GetUserId()
    {
        if (UserUid == null)
            throw new InvalidOperationException("UserUid is null. Пользователь не аутентифицирован.");
        return UserUid.Value;
    }
}
