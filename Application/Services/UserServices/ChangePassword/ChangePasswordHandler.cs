using Domain.Results;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Repository;

namespace Application.Services.UserServices.ChangePassword
{
    /// <summary>
    /// Change Password event handler
    /// </summary>
    public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

        public ChangePasswordHandler(IUserRepository userRepository, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByLoginAsync(request.Login);

            if (user == null)
            {
                return Result.CreateFailure(Error.BadRequest, "Пользователь не найден");
            }

            var passwordCheck = await _userRepository.CheckPasswordAsync(user, request.CurrentPassword);

            if (!passwordCheck)
            {
                return Result.CreateFailure(Error.BadRequest, "Неправильный пароль");
            }

            var result = await _userRepository.ValidateAsync(user, request.NewPassword);

            if (result.Succeeded)
            {
                user.PasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);

                await _userRepository.UpdateUserAsync(user);

                return Result.CreateSuccess("Пароль успешно изменен");
            }
            else
            {
                return Result.CreateFailure(Error.BadRequest, "Ошибка валидации пароля");
            }
        }
    }
}
