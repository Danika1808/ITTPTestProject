using Domain.Results;
using Application.Services.JWTServices.CreateAuthToken;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.JWTServices.AuthorizationUser
{
    public class AuthUserCommand : IRequest<Result<CreateTokenCommand>>
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
