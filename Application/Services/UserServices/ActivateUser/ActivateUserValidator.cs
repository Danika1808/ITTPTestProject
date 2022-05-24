using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.UserServices.ActivateUser
{
    public class ActivateUserValidator : AbstractValidator<ActivateUserCommand>
    {
        public ActivateUserValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
