using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace JwtBearerBreakingChange.Jwt;

internal class CustomJwtTokenHandler : JwtSecurityTokenHandler
{
    protected override JwtSecurityToken ValidateSignature(
        string token, TokenValidationParameters validationParameters)
    {
        var jwt = ReadJwtToken(token);

        if (!_verify(jwt))
        {
            throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidSignatureException("Signature did not match with token contents."));
        }

        return jwt;
    }

    private bool _verify(JwtSecurityToken jwt)
    {
        var headerAndPayload = $"{jwt.RawHeader}.{jwt.RawPayload}";
        var headerAndPayloadBytes = Encoding.UTF8.GetBytes(headerAndPayload);
        var signatureBytes = Base64UrlEncoder.DecodeBytes(jwt.RawSignature);

        using (var rsa = RSA.Create())
        {
            rsa.ImportFromPem(MySuperKey.PRIVATE_KEY);

            return rsa.VerifyData(headerAndPayloadBytes, signatureBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
    }
}
