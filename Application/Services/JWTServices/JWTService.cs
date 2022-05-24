using Application.Extensions;
using Domain.Results;
using Application.Services.JWTServices.AuthorizationUser;
using Application.Services.JWTServices.CreateAuthToken;
using Application.Services.JWTServices.RefreshAuthToken;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.JWTServices
{
    public class JWTService : IJWTService
    {
        private readonly IMediator _mediator;
        private readonly ILogger<JWTService> _logger;
        public JWTService(IMediator mediator, ILogger<JWTService> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        public async Task<Result<TokenResult>> CreateToken(CreateTokenCommand request)
        {
            _logger.LogEnter();
            var result = await _mediator.Send(request);
            _logger.LogExit(result);

            return result;
        }

        public async Task<Result<CreateTokenCommand>> RefreshToken(RefreshTokenCommand request)
        {
            _logger.LogEnter();
            var result = await _mediator.Send(request);
            _logger.LogExit(result);

            return result;
        }

        public async Task<Result<CreateTokenCommand>> SignInAsync(AuthUserCommand request)
        {
            _logger.LogEnter();
            var result = await _mediator.Send(request);
            _logger.LogExit(result);

            return result;
        }
    }
}
