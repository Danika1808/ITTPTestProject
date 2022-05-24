using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.JWTServices.AuthorizationUser
{
    internal class AuthUserValidator : AbstractValidator<AuthUserCommand>
    {
        public AuthUserValidator()
        {
            RuleFor(x => x.Login).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
