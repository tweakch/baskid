using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
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

            /*
                Shows how to add custom Command

                baskid.Command("get", command =>
                {
                    var urlArgument = command.Argument("[url]", "the url to get data from");
                    command.OnExecute(async () =>
                    {
                        var repositories = await ProcessRepositories(urlArgument.Value);
                        foreach (var repo in repositories)
                        {
                            Console.WriteLine(repo.Name);
                            Console.WriteLine(repo.Description);
                            Console.WriteLine(repo.GitHubHomeUrl);
                            Console.WriteLine(repo.Homepage);
                            Console.WriteLine(repo.Watchers);
                            Console.WriteLine(repo.LastPush);
                            Console.WriteLine();
                        }
                        return 0;
                    });
                });
            */

            ILogger logger = serviceProvider.GetService<ILogger<Program>>();
            try
            {
                logger.LogTrace(args.ToJson());
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
            return new ServiceCollection()
                    .AddLogging(cfg => {
                        cfg.AddConsole();
                        cfg.SetMinimumLevel(LogLevel.Trace);
                    })
                    .AddDbContext<BaskidContext>(options => options.UseSqlite("DataSource=:memory:"))
                    .AddBaskid()
.BuildServiceProvider();
        }

        private static async Task<List<Repository>> ProcessRepositories(string url = "https://api.github.com/orgs/dotnet/repos")
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            var streamTask = client.GetStreamAsync(url);
            var repositories = await JsonSerializer.DeserializeAsync<List<Repository>>(await streamTask);
            return repositories;
        }
    }
}