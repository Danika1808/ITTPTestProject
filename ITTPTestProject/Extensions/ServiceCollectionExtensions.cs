using AutoMapper;
using ITTPTestProject.Mapper;

namespace ITTPTestProject.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMapper(this IServiceCollection services)
        {
            var mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile<UserProfile>();
            });

            IMapper mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
