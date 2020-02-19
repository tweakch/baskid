using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Baskid.Core.Provider;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace Baskid.Core
{
    public class CoreModule : ABaskidModule
    {
        private readonly ILogger<CoreModule> _logger;
        private readonly IQueryStoreManager _manager;

        public CoreModule(ILogger<CoreModule> logger, IQueryStoreManager manager)
        {
            _logger = logger;
            _manager = manager;
            Commands = new Dictionary<string, Action<CommandLineApplication>>();
            Commands.Add("init", InitCommand);
            Commands.Add("status", StatusCommand);
            Commands.Add("add", AddCommand);
        }

        public override Dictionary<string, Action<CommandLineApplication>> Commands { get; set; }

        private void InitCommand(CommandLineApplication command)
        {
            command.Description = "Initialize baskid repository";
            command.OnExecute(() =>
            {
                if (!Directory.Exists(BasikdDirectoryName))
                {
                    var di = Directory.CreateDirectory(BasikdDirectoryName);
                    di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                }

                var configFilePath = Path.Combine(BasikdDirectoryName, "config");
                if (!File.Exists(configFilePath))
                {
                    using (var stream = File.Create(configFilePath))
                    {
                        var config = new BaskidRepositoryConfiguration();
                        config.DatabaseProvider = "sqlite";
                        stream.Write(Encoding.Default.GetBytes(config.ToJson()));
                        stream.Flush();
                    }
                    _logger.LogInformation($"Initialized empty baskid repository in {Path.Combine(Directory.GetCurrentDirectory(), ".baskid")}");
                }
                else
                {
                    Console.WriteLine($"Reinitialized existing baskid repository in {Path.Combine(Directory.GetCurrentDirectory(), ".baskid")}");
                }
                return 0;
            });
        }

        private void StatusCommand(CommandLineApplication command)
        {
            command.Description = "Display status";
            command.OnExecute(() =>
            {
                Console.WriteLine("not a baskid repository");
                return 0;
            });
        }

        private void AddCommand(CommandLineApplication command)
        {
            command.HelpOption("-?|--help");
            command.Argument("[query]", "The query to be added", config =>
            {
                config.ShowInHelpText = true;
            }, true);

            command.Description = "Add entry";
            command.OnExecute(() =>
            {
                if (!IsBaskidRepository) throw new InvalidOperationException("Not a baskid repository");

                var values = command.Arguments.Select(ca => ca.Value);
                var id = _manager.Add(string.Join(" ", values));
                _logger.LogInformation($"Added query {id} => {values.ToJson()}");
                return 0;
            });
        }
    }
}