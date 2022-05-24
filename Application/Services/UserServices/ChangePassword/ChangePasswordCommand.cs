using Domain.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Application.Services.UserServices.ChangePassword
{
    /// <summary>
    /// Change password request
    /// </summary>
    public class ChangePasswordCommand : IRequest<Result>
    {
        public string Login { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }

    }
}
