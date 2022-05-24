using Domain.Results;
using Application.Services.JWTServices.CreateAuthToken;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.JWTServices.RefreshAuthToken
{
    public class RefreshTokenCommand : IRequest<Result<CreateTokenCommand>>
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
