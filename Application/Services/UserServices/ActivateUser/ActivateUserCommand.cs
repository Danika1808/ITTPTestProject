using Domain.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.UserServices.ActivateUser
{
    public class ActivateUserCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
    }
}
