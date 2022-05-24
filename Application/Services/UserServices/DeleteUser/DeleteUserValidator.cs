using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.UserServices.DeleteUser
{
    public class DeleteUserValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserValidator()
        {
            RuleFor(x => x.TargetUserId).NotEmpty();
        }
    }
}
