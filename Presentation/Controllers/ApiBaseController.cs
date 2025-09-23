using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
public abstract class ApiBaseController : ControllerBase
{
    protected Guid? UserUid => !User.Identity.IsAuthenticated
        ? null
        :Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
    protected Guid GetUserId() => UserUid.Value;
}
