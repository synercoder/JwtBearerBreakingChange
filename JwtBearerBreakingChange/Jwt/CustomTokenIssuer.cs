using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JwtBearerBreakingChange.Jwt;

public class CustomTokenIssuer
{
    public const string KID = "SomethingSomethingDarkside.";

    public async Task<IssuedTokenDto> IssueAsync()
    {
        var jwtId = Guid.NewGuid();
        var expiresOn = DateTime.UtcNow.Add(TimeSpan.FromHours(10));

        var header = _makeHeader();
        var payload = _makePayload(jwtId, expiresOn);
        var signature = await _makeSignatureAsync($"{header}.{payload}");

        return new IssuedTokenDto
        {
            Jwt = $"{header}.{payload}.{signature}",
            ExpiresOn = expiresOn
        };
    }

    private string _makeHeader()
    {
        var headerJson = JsonConvert.SerializeObject(new Dictionary<string, string>
        {
            [JwtHeaderParameterNames.Typ] = "JWT",
            [JwtHeaderParameterNames.Alg] = "RS256",
            [JwtHeaderParameterNames.Kid] = KID
        });

        return Base64UrlEncoder.Encode(headerJson);
    }

    private string _makePayload(Guid jwtId, DateTime expiresOn)
    {
        var claims = new Claim[]
        {
            new(JwtRegisteredClaimNames.Jti, jwtId.ToString())
        };

        return new JwtSecurityToken("MyCustomApp", claims: claims, expires: expiresOn)
            .EncodedPayload;
    }

    private async Task<string> _makeSignatureAsync(string headerAndPayload)
    {
        using (var rsa = RSA.Create())
        {
            rsa.ImportFromPem(MySuperKey.PRIVATE_KEY);

            var signature = rsa.SignData(Encoding.UTF8.GetBytes(headerAndPayload), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            return Base64UrlEncoder.Encode(signature);
        }
    }

    public class IssuedTokenDto
    {
        public string? Jwt { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}
