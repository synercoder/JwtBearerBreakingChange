using Microsoft.IdentityModel.Tokens;

namespace JwtBearerBreakingChange.Jwt;

public static class CustomJwtBearerDefaults
{
    public const string SCHEME = "CustomJwtBearer";

    public static TokenValidationParameters TokenValidationParameters = new()
    {
        RequireAudience = false,
        RequireExpirationTime = true,

        RequireSignedTokens = false,

        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
    };
}
