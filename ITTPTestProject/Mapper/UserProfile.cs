using Application.Services.UserServices.CreateUser;
using AutoMapper;
using Domain;
using Domain.Dto;

namespace ITTPTestProject.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserCommand, ApplicationUser>()
                .ForMember(x => x.CreatedOn, options => options.MapFrom(x => DateTime.UtcNow))
                .ForMember(x => x.UserName, options => options.MapFrom(x => x.Login))
                .ForMember(x => x.Gender, options => options.MapFrom(x => (int)x.Gender));
            CreateMap<ApplicationUser, UserDto>();
        }
    }
}
