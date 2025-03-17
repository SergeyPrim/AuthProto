using AuthProto.Business.Settings;
using AuthProto.Shared.DI;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthProto.Business.General.Utilities
{
    public interface IJwtService
    {
        public class IssueJwtRequest
        {
            public Guid UserId { get; set; }
        }

        public class IssueJwtResponse
        {
            public string Token { get; set; }
            public DateTime ExpiresInUtc { get; set; }
        }

        IssueJwtResponse IssueJwt(IssueJwtRequest request);
    }

    [TransientRegistration]
    class JwtService : IJwtService
    {
        readonly JwtSecuritySettings _settings;

        public JwtService(IOptions<JwtSecuritySettings> settings)
        {
            _settings = settings.Value;
        }

        public IJwtService.IssueJwtResponse IssueJwt(IJwtService.IssueJwtRequest request)
        {
            var data = Convert.FromBase64String(_settings.JwtKey);
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(data);

            var claims = new Dictionary<string, object>
            {
                [ClaimTypes.NameIdentifier] = request.UserId,
            };
            var descriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
            {
                Issuer = _settings.Issuer,
                Audience = _settings.Audience,
                Claims = claims,
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddHours(_settings.ValidPeriodInHours),
                SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)
            };

            var handler = new Microsoft.IdentityModel.JsonWebTokens.JsonWebTokenHandler();
            handler.SetDefaultTimesOnTokenCreation = false;
            var tokenString = handler.CreateToken(descriptor);

            return new()
            {
                Token = tokenString,
                ExpiresInUtc = descriptor.Expires.Value
            };
        }
    }
}
