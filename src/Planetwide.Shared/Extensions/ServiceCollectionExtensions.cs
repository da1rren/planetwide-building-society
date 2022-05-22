namespace Planetwide.Shared.Extensions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterRedis(this IServiceCollection services)
    {
        return services.AddSingleton(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var connectionString = config["Database:Redis"];
            ArgumentNullException.ThrowIfNull(connectionString);
            return ConnectionMultiplexer.Connect(connectionString);
        });
    }
}