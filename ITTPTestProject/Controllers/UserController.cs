using Domain.Results;
using Application.Services.UserServices;
using Application.Services.UserServices.ActivateUser;
using Application.Services.UserServices.ChangePassword;
using Application.Services.UserServices.CreateUser;
using Application.Services.UserServices.DeleteUser;
using Application.Services.UserServices.ReadUser;
using Application.Services.UserServices.UpdateUser;
using Domain;
using Domain.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ITTPTestProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = Constants.AdminRole)]
        [HttpPost("CreateUser")]
        public async Task<ActionResult<ResponseResult<UserDto>>> CreateUser([FromBody] CreateUserCommand request)
        {
            var currentUserLogin = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Login").Value;

            request.CreatedBy = currentUserLogin;

            var result = await _userService.CreateUserAsync(request);

            if (result.IsSucceess)
            {
                return GetResponse(result);
            }
            else
            {
                return GetErrorResponse(result);

            }
        }
        [Authorize]
        [HttpGet("/{login}")]
        public async Task<ActionResult<ResponseResult<UserDto>>> ReadUser([FromRoute] string login)
        {
            var currentUserLogin = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Login").Value;

            if (!User.IsInRole("Admin") && login != currentUserLogin)
            {
                var localResult = Result.CreateFailure(Error.Forbidden, "Доступ запрещен");

                return GetErrorResponse<UserDto>(localResult);
            }

            var query = new ReadUserQuery
            {
                Login = login
            };

            var result = await _userService.ReadUserAsync(query);

            if (result.Value.Count == 0)
            {
                var localResult = Result.CreateFailure(Error.NotFound, "Пользователь не найден");
                return GetErrorResponse<UserDto>(localResult);

            }
            else
            {
                var user = result.Value.First();
                var localResult = Result<UserDto>.CreateSuccess(user);
                return GetResponse(localResult);
            }
        }

        [Authorize(Roles = Constants.AdminRole)]
        [HttpGet("ReadUsers")]
        public async Task<ActionResult<ResponseResult<List<UserDto>>>> ReadUser([FromQuery]ReadUserQuery request)
        {
            request.IsAdmin = true;
            var result = await _userService.ReadUserAsync(request);

            return GetResponse(result);
        }

        [Authorize]
        [HttpPatch("UpdateUser")]
        public async Task<ActionResult<ResponseResult<UserDto>>> UpdateUser([FromQuery][Required]Guid userId, [FromBody] JsonPatchDocument<ApplicationUser> jsonPatch)
        {
            var currentUserId = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);

            if (!User.IsInRole("Admin") && userId != currentUserId)
            {
                var localResult = Result.CreateFailure(Error.Forbidden, "Доступ запрещен");
                return GetErrorResponse<UserDto>(localResult);
            }

            var currentUserLogin = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Login").Value;

            var command = new UpdateUserCommand()
            {
                Id = userId,
                JsonPatch = jsonPatch,
                ModifiedBy = currentUserLogin,
                IsAdmin = User.IsInRole("Admin")
            };

            var result = await _userService.UpdateUserAsync(command);

            if (result.IsSucceess)
            {
                return GetResponse(result);
            }
            else
            {
                return GetErrorResponse(result);

            }
        }

        [Authorize(Roles = Constants.AdminRole)]
        [HttpDelete("DeleteUser")]
        public async Task<ActionResult<ResponseResult>> DeleteUser([FromBody] DeleteUserCommand request)
        {
            request.RevokedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Login").Value;
            var result = await _userService.DeleteUserAsync(request);

            if (result.IsSucceess)
            {
                return GetResponse(result);
            }
            else
            {
                return GetErrorResponse(result);
            }
        }

        [Authorize(Roles = Constants.AdminRole)]
        [HttpPost("ActivateUser")]
        public async Task<ActionResult<ResponseResult>> ActivateUser([FromBody] ActivateUserCommand request)
        {
            var result = await _userService.ActivateUserAsync(request);

            if (result.IsSucceess)
            {
                return GetResponse(result);
            }
            else
            {
                return GetErrorResponse(result);
            }
        }

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<ActionResult<ResponseResult>> ChangePassword([FromBody] ChangePasswordCommand request)
        {
            var currentUserLogin = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Login").Value;

            if (!User.IsInRole("Admin") && request.Login != currentUserLogin)
            {
                var localResult = Result.CreateFailure(Error.Forbidden, "Доступ запрещен");

                return GetErrorResponse(localResult);
            }

            var result = await _userService.ChangePassword(request);

            if (result.IsSucceess)
            {
                return GetResponse(result);
            }
            else
            {
                return GetErrorResponse(result);
            }
        }
    }
}
