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


namespace Baskid
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
            .AddLogging()
            .AddDbContext<BaskidContext>(options => options.UseSqlite("DataSource=:memory:"))
            .AddBaskid()
            .BuildServiceProvider();
            
            var baskid = serviceProvider.GetService<CommandLineApplication>();

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

            try
            {
                baskid.Execute(args);
            }
            catch (System.Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }

        private static async Task<List<Repository>> ProcessRepositories(string url = "https://api.github.com/orgs/dotnet/repos")
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            var streamTask = client.GetStreamAsync(url);
            var repositories = await JsonSerializer.DeserializeAsync<List<Repository>>(await streamTask);
            return repositories;
        }
    }
}