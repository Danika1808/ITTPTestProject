using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace Application.Extensions
{
    /// <summary>
    /// IServiceCollection extensions class
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Inject all Mediators from this project
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMediators(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }

        /// <summary>
        /// Inject all Validators from this project
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
