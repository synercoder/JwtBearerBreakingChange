using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace JwtBearerBreakingChange.Jwt;

public class CustomJwtBearerHandler : JwtBearerHandler
{
    public CustomJwtBearerHandler(
        IOptionsMonitor<JwtBearerOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
            : base(options, logger, encoder, clock)
    { }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var result = await base.HandleAuthenticateAsync();

        if (!result.Succeeded)
        {
            Logger.LogWarning(result.Failure, "Authentication using CustomJwtBearerHandler failed.");
            return result;
        }

        return result;
    }

    protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        var authResult = await HandleAuthenticateOnceSafeAsync();

        if (!authResult.Succeeded)
        {
            await base.HandleChallengeAsync(properties);
        }
    }
}
