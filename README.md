# Breaking change in System.IdentityModel.Tokens.Jwt

If you upgrade Microsoft.AspNetCore.Authentication.JwtBearer from 6.0.25 to 6.0.26,
the transient dependency on System.IdentityModel.Tokens.Jwt will update from 6.10.0 ot 6.35.0, 
which should be minor, but are breaking:

Microsoft.AspNetCore.Authentication.JwtBearer 6.0.26
 => Microsoft.IdentityModel.Protocols.OpenIdConnect 6.35.0
     => System.IdentityModel.Tokens.Jwt 6.35.0,
Microsoft.AspNetCore.Authentication.JwtBearer 6.0.25 
 => Microsoft.IdentityModel.Protocols.OpenIdConnect 6.10.0
     => System.IdentityModel.Tokens.Jwt 6.10.0
     
In `System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler` the method `protected virtual JwtSecurityToken ValidateSignature(string token, TokenValidationParameters validationParameters)` was used to verify the signature in the 
`public override ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters, out SecurityToken validatedToken)` method. This method is since rewritten/refactored, to no longer use the previously mentioned `ValidateSignature`.

Thus sub-class implementations of JwtSecurityTokenHandler that only override `JwtSecurityToken ValidateSignature(string token, TokenValidationParameters validationParameters)` will no longer work.