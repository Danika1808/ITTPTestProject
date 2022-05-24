using Application.Extensions;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Infrastructure.Repository;

namespace Application.Services.UserServices.UpdateUser
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Result<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UpdateUserHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Result<UserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserAsync(request.Id);

            if (user == null)
            {
                return Result<UserDto>.CreateFailure(Error.NotFound, "Пользователь не найден");
            }

            var previousIsRevoked = user.IsRevoked;

            request.JsonPatch.Sanitize();

            var paths = request.JsonPatch.Operations.Select(x => x.path.Split("/", StringSplitOptions.RemoveEmptyEntries).FirstOrDefault().ToLower());

            if (!paths.Any())
            {
                return Result<UserDto>.CreateFailure(Error.BadRequest, "Ошибка при попытке изменить сущность");
            }

            try
            {
                request.JsonPatch.ApplyTo(user);
            }
            catch
            {
                return Result<UserDto>.CreateFailure(Error.BadRequest, "Ошибка при попытке изменить сущность");

            }

            foreach (var item in paths)
            {
                var localResult = item switch
                {
                    "gender" => GenderPropertyUpdate(user),
                    "name" => NamePropertyUpdate(user),
                    "login" => await LoginPropertyUpdateAsync(user),
                    "isadmin" => await IsAdminPropertyUpdateAsync(request, user),
                    _ => null
                };

                if (localResult != null)
                {
                    return localResult;
                }
            }

            user.ModifiedOn = DateTime.UtcNow;

            if (request.IsAdmin)
            {
                user.ModifiedBy = request.ModifiedBy;
            }
            else
            {
                user.ModifiedBy = user.Login;
            }

            await _userRepository.UpdateUserAsync(user);

            var result = _mapper.Map<UserDto>(user);

            return Result<UserDto>.CreateSuccess(result);
        }

        public static Result<UserDto> GenderPropertyUpdate(ApplicationUser user)
        {
            var result = Enum.TryParse($"{user.Gender}", true, out Gender parsedEnumValue) && Enum.IsDefined(typeof(Gender), parsedEnumValue);
            if (!result)
            {
                return Result<UserDto>.CreateFailure(Error.BadRequest, "Пол указан неверно");
            }

            return null;
        }

        public static Result<UserDto> NamePropertyUpdate(ApplicationUser user)
        {
            if (string.IsNullOrEmpty(user.Name))
            {
                return Result<UserDto>.CreateFailure(Error.BadRequest, "Имя невалидно");
            }
            if (!Regex.IsMatch(user.Name, Constants.LatinAndRussianLettersRegex))
            {
                return Result<UserDto>.CreateFailure(Error.BadRequest, "Имя невалидно");
            }

            return null;
        }

        public async Task<Result<UserDto>> LoginPropertyUpdateAsync(ApplicationUser user)
        {
            if (string.IsNullOrEmpty(user.Login))
            {
                return Result<UserDto>.CreateFailure(Error.BadRequest, "Логин невалиден");
            }
            if (!Regex.IsMatch(user.Login, Constants.LatinAndNumberLettersRegex))
            {
                return Result<UserDto>.CreateFailure(Error.BadRequest, "Логин невалиден");
            }
            if (await _userRepository.AnyAsync(user.Login))
            {
                return Result<UserDto>.CreateFailure(Error.Conflict, "Пользователь с таким логином уже существует");
            }

            user.UserName = user.Login;

            return null;
        }

        public async Task<Result<UserDto>> IsAdminPropertyUpdateAsync(UpdateUserCommand request, ApplicationUser user)
        {
            if (!request.IsAdmin)
            {
                return Result<UserDto>.CreateFailure(Error.Forbidden, "Нет доступа");
            }

            if (user.IsAdmin)
            {
                var addRoleResult = await _userRepository.AddToRoleAsync(user, Constants.AdminRole);

                if (!addRoleResult.Succeeded)
                {
                    return Result<UserDto>.CreateFailure(Error.InternalServerError, "Ошибка добавление роли");
                }
            }
            else
            {
                var addRoleResult = await _userRepository.RemoveFromRoleAsync(user, Constants.AdminRole);

                if (!addRoleResult.Succeeded)
                {
                    return Result<UserDto>.CreateFailure(Error.InternalServerError, "Ошибка удаление роли");
                }
            }

            return null;
        }
    }
}
