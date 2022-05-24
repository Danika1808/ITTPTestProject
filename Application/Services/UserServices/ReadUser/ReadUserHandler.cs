using Domain.Results;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using Domain.Dto;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.UserServices.ReadUser
{
    public class ReadUserHandler : IRequestHandler<ReadUserQuery, Result<List<UserDto>>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ReadUserHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<List<UserDto>>> Handle(ReadUserQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Users.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(request.Login) && request.IsAdmin)
            {
                var localResult = await query.IgnoreQueryFilters().ProjectTo<UserDto>(_mapper.ConfigurationProvider).Where(x => x.Login == request.Login).ToListAsync(cancellationToken: cancellationToken);
                
                return Result<List<UserDto>>.CreateSuccess(localResult);
            }

            if (!string.IsNullOrEmpty(request.Login))
            {
                var localResult = await query.ProjectTo<UserDto>(_mapper.ConfigurationProvider).Where(x => x.Login == request.Login).ToListAsync(cancellationToken: cancellationToken);
               
                return Result<List<UserDto>>.CreateSuccess(localResult);
            }

            if (request.Age != default)
            {
                var targetDate = DateTime.UtcNow.AddYears(-request.Age);

                query = query.Where(x => x.Birthday < targetDate);
            }

            if (request.IsRevokedIgnore)
            {
                query = query.IgnoreQueryFilters();
            }

            if (request.RevokedOnly)
            {
                query = query.IgnoreQueryFilters().Where(x => x.IsRevoked == true);
            }

            if (request.Order == Order.desc)
            {
                query = query.OrderByDescending(x => x.CreatedOn);
            }

            if (request.Order == Order.acs)
            {
                query = query.OrderBy(x => x.CreatedOn);
            }

            var result = await query.ProjectTo<UserDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken: cancellationToken);

            return Result<List<UserDto>>.CreateSuccess(result);
        }
    }
}
