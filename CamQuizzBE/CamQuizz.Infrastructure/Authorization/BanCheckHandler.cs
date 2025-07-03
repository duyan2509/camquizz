using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using CamQuizz.Persistence;

public class BanCheckHandler : AuthorizationHandler<NotBannedRequirement>
{
    private readonly ApplicationDbContext _db;

    public BanCheckHandler(ApplicationDbContext db)
    {
        _db = db;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, NotBannedRequirement requirement)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return;
        Guid userGuid = Guid.Parse(userId);
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userGuid);
        if (user == null || !user.IsActive)
            return;

        context.Succeed(requirement);
    }
}