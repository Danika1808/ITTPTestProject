using Application.Extensions;
using FluentValidation;

namespace Application.Services.UserServices.ChangePassword
{
    /// <summary>
    /// Change password Command Validator
    /// </summary>
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.Login).NotEmpty();
            RuleFor(x => x.CurrentPassword).NotEmpty();
            RuleFor(x => x.NewPassword).Password();
            RuleFor(x => x.ConfirmNewPassword).Equal(x => x.NewPassword).WithMessage("Пароли не совпадают");
        }
    }
}
