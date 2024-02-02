using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace JwtBearerBreakingChange.Jwt;

internal class CustomJwtBearerPostConfigureOptions : IPostConfigureOptions<JwtBearerOptions>
{
    private readonly CustomJwtTokenHandler _validator;

    public CustomJwtBearerPostConfigureOptions(CustomJwtTokenHandler validator)
    {
        _validator = validator;
    }

    public void PostConfigure(string name, JwtBearerOptions options)
    {
        options.SecurityTokenValidators.Clear();
        options.SecurityTokenValidators.Add(_validator);

        options.TokenValidationParameters.SignatureValidator = _validator.SignatureValidatorHook;
    }
}
