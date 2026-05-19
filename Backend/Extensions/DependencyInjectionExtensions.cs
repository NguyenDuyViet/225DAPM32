using System.Reflection;

namespace Backend.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            var repositoryTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type =>
                    type.IsClass &&
                    !type.IsAbstract &&
                    type.Name.EndsWith("Repository")
                );

            foreach (var implementationType in repositoryTypes)
            {
                services.AddScoped(implementationType);
            }

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var serviceTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type =>
                    type.IsClass &&
                    !type.IsAbstract &&
                    type.Name.EndsWith("Service")
                );

            foreach (var implementationType in serviceTypes)
            {
                services.AddScoped(implementationType);
            }

            return services;
        }
    }
}