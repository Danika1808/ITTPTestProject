using Domain.Results;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.JWTServices.CreateAuthToken
{
    public class CreateTokenCommand : IRequest<Result<TokenResult>>
    {
        public string UserName { get; set; }
        public Guid UserId { get;set; }
        public IList<string> Roles { get; set; }
    }
}
