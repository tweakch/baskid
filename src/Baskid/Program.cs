using System;
using System.IO;
using Baskid.Core;
using Baskid.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Baskid
{
    internal class Program
    {
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
                Console.Error.WriteLine(e.Message);
                logger.LogError(e.Message);
            }
            finally
            {
                serviceProvider.Dispose();
            }
        }

        private static ServiceProvider RegisterServices(string[] args)
        {
            //var configuration = new ConfigurationBuilder();
            //Configuration = configuration.SetBasePath(Directory.GetCurrentDirectory())
            //                             .AddJsonFile("appsettings.json", false, true)
            //                             .AddCommandLine(args)
            //                             .Build();

            var services = new ServiceCollection();
            return services.AddDbContext<BaskidContext>(options =>
                           {
                               options.UseSqlite("DataSource=.baskid\\data");
                           })
                           .AddBaskid()
                           .AddLogging(cfg =>
                           {
                               //cfg.AddConfiguration(configuration);
                               //cfg.SetMinimumLevel(LogLevel.Debug);
                           })
                           .BuildServiceProvider();
        }
    }
}