using Domain.Results;
using Domain;
using Domain.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace Application.Services.UserServices.CreateUser
{
    public class CreateUserCommand : IRequest<Result<UserDto>>
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public bool IsAdmin { get; set; }
        [BindNever]
        [JsonIgnore]
        public string? CreatedBy { get; set; }
    }
}