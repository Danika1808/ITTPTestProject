using Domain.Results;
using Application.Services.JWTServices.CreateAuthToken;
using Domain;
using Infrastructure;
using Infrastructure.Repository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.JWTServices.RefreshAuthToken
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, Result<CreateTokenCommand>>
    {
        private readonly TokenValidationParameters _tokenValidationParams;
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;

        public RefreshTokenHandler(TokenValidationParameters tokenValidationParams, IUserRepository userRepository, ITokenRepository tokenRepository)
        {
            _tokenValidationParams = tokenValidationParams;
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
        }

        public async Task<Result<CreateTokenCommand>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var tokenInVerification = jwtTokenHandler.ValidateToken(request.Token, _tokenValidationParams, out var validatedToken);

            if (validatedToken == null)
            {
                return Result<CreateTokenCommand>.CreateFailure(Error.BadRequest, "Invalid token");

            }

            var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

            if (expiryDate > DateTime.UtcNow)
            {
                return Result<CreateTokenCommand>.CreateFailure(Error.BadRequest, "Token has not yet expired");
            }

            var storedToken = await _tokenRepository.GetRefreshTokenAsync(validatedToken.Id);

            if (storedToken == null)
            {
                return Result<CreateTokenCommand>.CreateFailure(Error.NotFound, "Token does not exist");
            }

            if (storedToken.IsUsed)
            {
                return Result<CreateTokenCommand>.CreateFailure(Error.BadRequest, "Token has been used");
            }

            if (storedToken.IsRevorked)
            {
                return Result<CreateTokenCommand>.CreateFailure(Error.BadRequest, "Token has been revoked");
            }

            var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            if (storedToken.JwtId != jti)
            {
                return Result<CreateTokenCommand>.CreateFailure(Error.BadRequest, "Token doesn't match");
            }

            storedToken.IsUsed = true;

            await _tokenRepository.UpdateTokenAsync(storedToken);

            var user = await _userRepository.GetUserAsync(storedToken.UserId);

            var roles = await _userRepository.GetRolesAsync(user);

            var createTokenCommand = new CreateTokenCommand
            {
                Roles = roles,
                UserId = user.Id,
                UserName = user.UserName
            };
            return Result<CreateTokenCommand>.CreateSuccess(createTokenCommand);
        }

        private static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();

            return dateTimeVal;
        }
    }
}
