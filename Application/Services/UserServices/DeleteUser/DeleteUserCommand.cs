using Domain.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Services.UserServices.DeleteUser
{
    public class DeleteUserCommand : IRequest<Result>
    {
        public Guid TargetUserId { get; set; }
        public bool IsSoftDelete { get; set; }
        [BindNever]
        [JsonIgnore]
        public string? RevokedBy { get; set; }
    }
}
