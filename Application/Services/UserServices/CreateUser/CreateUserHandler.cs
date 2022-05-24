using Domain.Results;
using AutoMapper;
using Domain;
using Domain.Dto;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Repository;

namespace Application.Services.UserServices.CreateUser
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, Result<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public CreateUserHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Result<UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            DateTime? birthday = null;

            if (request.Day != default && request.Month != default && request.Year != default)
            {
                birthday = new DateTime(request.Year, request.Month, request.Day);
            }
            else
            {
                if (request.Day != default || request.Month != default || request.Year != default)
                {
                    return Result<UserDto>.CreateFailure(Error.BadRequest, "Заполните дату рождения полностью");
                }
            }

            var user = await _userRepository.GetUserAsync(default, request.Login, true);

            if (user is not null)
            {
                return Result<UserDto>.CreateFailure(Error.Conflict, "Пользователь с таким логином уже существует");
            }

            user = _mapper.Map<ApplicationUser>(request);

            user.CreatedBy = request.CreatedBy;
            user.Birthday = birthday;

            var createUserResult = await _userRepository.CreateUserAsync(user, request.Password);

            if (!createUserResult.Succeeded)
            {
                return Result<UserDto>.CreateFailure(Error.InternalServerError, "Ошибка создания пользователя");
            }

            if (request.IsAdmin)
            {
                var addRoleResult = await _userRepository.AddToRoleAsync(user, Constants.AdminRole);

                if (!addRoleResult.Succeeded)
                {
                    return Result<UserDto>.CreateFailure(Error.InternalServerError, "Ошибка добавление роли");
                }
            }

            var result = _mapper.Map<UserDto>(user);

            return Result<UserDto>.CreateSuccess(result);
        }
    }
}
