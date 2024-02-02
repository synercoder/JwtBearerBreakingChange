using Microsoft.AspNetCore.Mvc;
using JwtBearerBreakingChange.Jwt;

namespace JwtBearerBreakingChange.Controllers;

[ApiController]
[Route("[controller]")]
public class TokenController : ControllerBase
{
    private readonly CustomTokenIssuer _tokenIssuer;

    public TokenController(CustomTokenIssuer tokenIssuer)
    {
        _tokenIssuer = tokenIssuer;
    }

    [HttpGet(Name = "Issue")]
    public async Task<CustomTokenIssuer.IssuedTokenDto> GetAsync()
    {
        return await _tokenIssuer.IssueAsync();
    }
}
