using Domain.Results;
using Application.Services.UserServices.ActivateUser;
using Application.Services.UserServices.ChangePassword;
using Application.Services.UserServices.CreateUser;
using Application.Services.UserServices.DeleteUser;
using Application.Services.UserServices.ReadUser;
using Application.Services.UserServices.UpdateUser;
using Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.UserServices
{
    public interface IUserService
    {
        Task<Result<UserDto>> CreateUserAsync(CreateUserCommand request);
        Task<Result<UserDto>> UpdateUserAsync(UpdateUserCommand request);
        Task<Result> DeleteUserAsync(DeleteUserCommand request);
        Task<Result<List<UserDto>>> ReadUserAsync(ReadUserQuery request);
        Task<Result> ChangePassword(ChangePasswordCommand request);
        Task<Result> ActivateUserAsync(ActivateUserCommand request);
    }
}
