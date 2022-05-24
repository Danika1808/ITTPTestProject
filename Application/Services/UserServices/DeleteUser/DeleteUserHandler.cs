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

namespace Application.Services.UserServices.DeleteUser
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Result>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserAsync(request.TargetUserId, isRevokedIgnore: true);

            if (user == null)
            {
                return Result.CreateFailure(Error.NotFound, "Пользователь не найден");
            }

            if (request.IsSoftDelete)
            {
                if (user.IsRevoked)
                {
                    return Result.CreateFailure(Error.Conflict, "Пользователь уже деактивирован");
                }
                user.IsRevoked = true;
                user.RevokedBy = request.RevokedBy;
                user.RevokedOn = DateTime.UtcNow;

                await _userRepository.UpdateUserAsync(user);

                return Result.CreateSuccess("Пользователь успешно деактивирован");

            }
            else
            {
                await _userRepository.DeleteUserAsync(user);

                return Result.CreateSuccess("Пользователь успешно удален");
            }
        }
    }
}
