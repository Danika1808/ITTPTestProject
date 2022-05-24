using Domain.Results;
using Domain;
using Infrastructure;
using Infrastructure.Repository;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.JWTServices.CreateAuthToken
{
    public class CreateTokenHandler : IRequestHandler<CreateTokenCommand, Result<TokenResult>>
    {
        private readonly SymmetricSecurityKey _key;
        private readonly ITokenRepository _tokenRepository;
        public CreateTokenHandler(IConfiguration config, ITokenRepository tokenRepository)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
            _tokenRepository = tokenRepository;
        }

        public async Task<Result<TokenResult>> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>()
            {
                new Claim("Login", request.UserName),
                new Claim("UserId", request.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in request.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddSeconds(Constants.TokenLifetimeOnSeconds),
                SigningCredentials = credentials,
                Audience = Constants.Audience,
                Issuer = Constants.Issuer,
                NotBefore = DateTime.UtcNow,

            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            var jwtToken = jwtTokenHandler.WriteToken(token);

            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                IsUsed = false,
                IsRevorked = false,
                UserId = request.UserId,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6),
                Token = RandomString(35) + Guid.NewGuid()
            };

            await _tokenRepository.AddTokenAsync(refreshToken);

            var authResult = new TokenResult()
            {
                Token = jwtToken,
                RefreshToken = refreshToken.Token
            };

            return Result<TokenResult>.CreateSuccess(authResult);
        }

        private static string RandomString(int length)
        {
            var random = new Random();
            var chars = Constants.Chars;
            return string.Join("", Enumerable.Repeat(chars, length).Select(x => x[random.Next(length)]));
        }
    }
}
