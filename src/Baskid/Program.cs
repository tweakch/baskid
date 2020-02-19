using System;
using System.Net.Http;
using Baskid.Core;
using Baskid.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Baskid
{
    internal class Program
    {
        private static readonly HttpClient client = new HttpClient();

        private static void Main(string[] args)
        {
            var serviceProvider = RegisterServices(args);
            var baskid = serviceProvider.GetService<CommandLineApplication>();

            ILogger logger = serviceProvider.GetService<ILogger<Program>>();
            try
            {
                baskid.Execute(args);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }
            finally
            {
                serviceProvider.Dispose();
            }
        }

        private static ServiceProvider RegisterServices(string[] args)
        {
            var services = new ServiceCollection();
            return services.AddLogging(cfg =>
                           {
                               cfg.AddConsole();
                               cfg.SetMinimumLevel(LogLevel.Trace);
                           })
                           .AddDbContext<BaskidContext>(options =>
                           {
                               options.UseSqlite("DataSource=:memory:");
                           })
                           .AddBaskid()
                           .BuildServiceProvider();
        }
    }
}