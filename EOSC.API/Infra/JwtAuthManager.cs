using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;

namespace EOSC.API.Infra;

public interface IJwtAuthManager
{
    JwtAuthResult GenerateToken(string username, DateTime now);
}

public class JwtAuthManager(JwtTokenConfig jwtTokenConfig) : IJwtAuthManager
{
    private readonly byte[] _secret = Encoding.ASCII.GetBytes(jwtTokenConfig.Secret);

    public JwtAuthResult GenerateToken(string username, DateTime now)
    {
        var shouldAddAudienceClaim = true;
        var jwtToken = new JwtSecurityToken(
            jwtTokenConfig.Issuer,
            shouldAddAudienceClaim ? jwtTokenConfig.Audience : string.Empty,
            expires: now.AddMinutes(jwtTokenConfig.AccessTokenExpiration),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(_secret),
                SecurityAlgorithms.HmacSha256Signature));
        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);


        return new JwtAuthResult
        {
            AccessToken = accessToken,
        };
    }

    public (ClaimsPrincipal, JwtSecurityToken?) DecodeJwtToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new SecurityTokenException("Invalid token");
        }

        var principal = new JwtSecurityTokenHandler()
            .ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtTokenConfig.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(_secret),
                    ValidAudience = jwtTokenConfig.Audience,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1)
                },
                out var validatedToken);
        return (principal, validatedToken as JwtSecurityToken);
    }
}

public class JwtAuthResult
{
    [JsonPropertyName("accessToken")] public string AccessToken { get; set; } = string.Empty;
}

public class RefreshToken
{
    [JsonPropertyName("username")]
    public string UserName { get; set; } = string.Empty; // can be used for usage tracking

    [JsonPropertyName("tokenString")] public string TokenString { get; set; } = string.Empty;

    [JsonPropertyName("expireAt")] public DateTime ExpireAt { get; set; }
}