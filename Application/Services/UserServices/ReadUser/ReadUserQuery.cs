using Domain.Results;
using Domain.Dto;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.UserServices.ReadUser
{
    public class ReadUserQuery : IRequest<Result<List<UserDto>>>
    {
        public string? Login { get; set; }
        public int Age { get; set; }
        public Order Order { get; set; }
        public bool IsRevokedIgnore { get; set; }
        public bool RevokedOnly { get; set; }
        [BindNever]
        public bool IsAdmin { get; set; }
    }

}
