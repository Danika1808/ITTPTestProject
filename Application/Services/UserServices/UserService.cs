using Application.Extensions;
using Domain.Results;
using Application.Services.JWTServices.AuthorizationUser;
using Application.Services.UserServices.ActivateUser;
using Application.Services.UserServices.ChangePassword;
using Application.Services.UserServices.CreateUser;
using Application.Services.UserServices.DeleteUser;
using Application.Services.UserServices.ReadUser;
using Application.Services.UserServices.UpdateUser;
using Domain.Dto;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserService> _logger;
        public UserService(IMediator mediator, ILogger<UserService> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Result> ActivateUserAsync(ActivateUserCommand request)
        {
            _logger.LogEnter();
            var result = await _mediator.Send(request);
            _logger.LogExit(result);

            return result;
        }

        public async Task<Result> ChangePassword(ChangePasswordCommand request)
        {
            _logger.LogEnter();
            var result = await _mediator.Send(request);
            _logger.LogExit(result);

            return result;
        }

        public async Task<Result<UserDto>> CreateUserAsync(CreateUserCommand request)
        {
            _logger.LogEnter();
           var result = await _mediator.Send(request);
            _logger.LogExit(result);

            return result;
        }

        public async Task<Result> DeleteUserAsync(DeleteUserCommand request)
        {
            _logger.LogEnter();
            var result = await _mediator.Send(request);
            _logger.LogExit(result);

            return result;
        }

        public async Task<Result<List<UserDto>>> ReadUserAsync(ReadUserQuery request)
        {
            _logger.LogEnter();
            var result = await _mediator.Send(request);
            _logger.LogExit(result);

            return result;
        }

        public async Task<Result<UserDto>> UpdateUserAsync(UpdateUserCommand request)
        {
            _logger.LogEnter();
            var result = await _mediator.Send(request);
            _logger.LogExit(result);

            return result;
        }
    }
}
