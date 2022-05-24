using Domain.Results;
using Infrastructure;
using Infrastructure.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.UserServices.ActivateUser
{
    public class ActivateUserHandler : IRequestHandler<ActivateUserCommand, Result>
    {
        private readonly IUserRepository _userRepository;

        public ActivateUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result> Handle(ActivateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserAsync(request.Id, isRevokedIgnore: true);

            if (user == null)
            {
                return Result.CreateFailure(Error.NotFound, "Пользователь не найден");
            }

            if (!user.IsRevoked)
            {
                return Result.CreateFailure(Error.Conflict, "Пользователь уже активирован");
            }

            user.IsRevoked = false;
            user.RevokedBy = default;
            user.RevokedOn = default;

            await _userRepository.UpdateUserAsync(user);

            return Result.CreateSuccess("Пользователь успешно активирован");
        }
    }
}
