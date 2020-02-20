// The code in this solution is awesome!

using System;
using System.Diagnostics;
using System.Text.Json;
using Baskid.Core.Module;
using Baskid.Core.Provider;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Baskid.Core
{
    public static class BaskidServiceCollectionExtensions
    {
        public static Action<TOption> GetSection<TOption>(this IConfiguration configuration, string key)
        {
            return (option) => configuration.GetValue<TOption>(key);
        }

        public static IServiceCollection AddBaskid(this IServiceCollection services)
        {
            services.AddScoped<IQueryStoreManager, QueryStoreManager>();
            services.AddScoped<IQueryProvider, QueryProvider>();
            services.AddScoped<CoreModule>();

            services.AddSingleton(service =>
            {
                var app = new CommandLineApplication();
                app.Name = "baskid";
                app.HelpOption("-?|-h|--help");

                // add module commands
                foreach (var command in service.GetRequiredService<CoreModule>().Commands)
                {
                    app.Command(command.Key, command.Value);
                }
                
                return app;
            });
            return services;
        }
    }
}