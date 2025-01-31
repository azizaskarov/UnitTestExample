using System.Reflection;
using UnitTestExample.Interfaces.ServiceLifeTimes;

namespace UnitTestExample.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddServicesAutomatically(this IServiceCollection services, Assembly assembly)
    {
        var types = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .ToList();

        foreach (var type in types)
        {
            var interfaces = type.GetInterfaces();

            foreach (var @interface in interfaces)
            {
                if (@interface == typeof(IScopedService))
                {
                    services.AddScoped(@interface, type);
                }
                else if (@interface == typeof(ISingletonService))
                {
                    services.AddSingleton(@interface, type);
                }
                else if (@interface == typeof(ITransientService))
                {
                    services.AddTransient(@interface, type);
                }
                else if (@interface == typeof(IHostedService)) 
                {
                    services.AddSingleton(@interface, type);  
                }
                else
                {
                    services.AddTransient(@interface, type); 
                }
            }
        }
    }
}

