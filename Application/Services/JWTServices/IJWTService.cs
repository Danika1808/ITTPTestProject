using Domain.Results;
using Application.Services.JWTServices.AuthorizationUser;
using Application.Services.JWTServices.CreateAuthToken;
using Application.Services.JWTServices.RefreshAuthToken;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.JWTServices
{
    public interface IJWTService
    {
        Task<Result<TokenResult>> CreateToken(CreateTokenCommand request);
        Task<Result<CreateTokenCommand>> SignInAsync(AuthUserCommand request);
        Task<Result<CreateTokenCommand>> RefreshToken(RefreshTokenCommand request);
    }
}
