using Domain.Results;
using Application.Services.JWTServices;
using Application.Services.JWTServices.AuthorizationUser;
using Application.Services.JWTServices.RefreshAuthToken;
using Microsoft.AspNetCore.Mvc;

namespace ITTPTestProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : BaseController
    {
        private readonly IJWTService _jWTService;

        public AuthorizationController(IJWTService jWTService)
        {
            _jWTService = jWTService;
        }

        [HttpPost("SignIn")]
        public async Task<ActionResult<ResponseResult<TokenResult>>> SignIn([FromBody]AuthUserCommand request)
        {
            var authUserResult = await _jWTService.SignInAsync(request);

            if (!authUserResult.IsSucceess)
            {
                return GetErrorResponse<TokenResult>(authUserResult);
            }

            var createTokenResult = await _jWTService.CreateToken(authUserResult.Value);

            if (!createTokenResult.IsSucceess)
            {
                return GetErrorResponse(createTokenResult);
            }

            return GetResponse(createTokenResult);
        }

        [HttpPost("RerfreshToken")]
        public async Task<ActionResult<ResponseResult<TokenResult>>> RerfreshToken([FromBody]RefreshTokenCommand request)
        {
            var refreshTokenResult = await _jWTService.RefreshToken(request);

            if (!refreshTokenResult.IsSucceess)
            {
                return GetErrorResponse<TokenResult>(refreshTokenResult);
            }

            var createTokenResult = await _jWTService.CreateToken(refreshTokenResult.Value);

            if (!createTokenResult.IsSucceess)
            {
                return GetErrorResponse(createTokenResult);
            }

            return GetResponse(createTokenResult);
        }
    }
}
