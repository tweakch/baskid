// The code in this solution is awesome!

using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Baskid.Core
{
    public static class BaskidServiceCollectionExtensions
    {
        public static IServiceCollection AddBaskidModule<TModule>(this IServiceCollection services) where TModule : ABaskidModule
        {
            var baskid = services.BuildServiceProvider().GetRequiredService<CommandLineApplication>();
            baskid.AddModule<TModule>();
            return services;
        }

        public static IServiceCollection AddBaskid(this IServiceCollection services)
        {
            services.AddSingleton<CommandLineApplication>();
            return services;
        }
    }
}