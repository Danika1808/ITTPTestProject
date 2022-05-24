using Domain.Results;
using Application.Services.JWTServices.CreateAuthToken;
using Domain;
using Infrastructure.Repository;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Services.JWTServices.AuthorizationUser
{
    public class AuthUserHandler : IRequestHandler<AuthUserCommand, Result<CreateTokenCommand>>
    {
        private readonly IUserRepository _userRepository;

        public AuthUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<CreateTokenCommand>> Handle(AuthUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByLoginAsync(request.Login);

            if (user == null)
            {
                return Result<CreateTokenCommand>.CreateFailure(Error.Unauthorized, "Пользователь не найден");
            }

            var result = await _userRepository.PasswordSignInAsync(user, request.Password, true, false);

            var roles = await _userRepository.GetRolesAsync(user);

            var createTokenCommand = new CreateTokenCommand
            {
                Roles = roles,
                UserId = user.Id,
                UserName = user.UserName
            };

            if (result.Succeeded)
            {
                return Result<CreateTokenCommand>.CreateSuccess(createTokenCommand);
            }
            else
            {
                return Result<CreateTokenCommand>.CreateFailure(Error.Forbidden, "Неправильный логин или пароль");
            }
        }
    }
}
