using Domain.Results;
using Domain;
using Domain.Dto;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Services.UserServices.UpdateUser
{
    public class UpdateUserCommand : IRequest<Result<UserDto>>
    {
        public Guid Id { get; set; }
        public JsonPatchDocument<ApplicationUser> JsonPatch { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsAdmin { get; set; }
    }
}
