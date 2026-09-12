using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GymTracker.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var value = user.FindFirstValue(JwtRegisteredClaimNames.Sub);

        return Guid.Parse(value!);
    }
}