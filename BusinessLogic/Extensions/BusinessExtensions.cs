using BusinessLogic.Contracts;
using BusinessLogic.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Repository.Extensions;

namespace BusinessLogic.Extensions
{
    public static class BusinessExtensions
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            // Register all business services
            services.AddScoped<ICharacterService, CharacterService>();

            // Include repository services
            services.AddRepositoryServices();

            return services;
        }
    }
}
