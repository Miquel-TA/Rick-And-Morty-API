using Microsoft.Extensions.DependencyInjection;
using Repository.Contracts;
using Repository.Implementations;

namespace Repository.Extensions
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
        {
            services.AddHttpClient<ICharacterRepository, CharacterRepository>(client =>
            {
                client.BaseAddress = new Uri("https://rickandmortyapi.com/api/");
            });
            return services;
        }
    }
}
