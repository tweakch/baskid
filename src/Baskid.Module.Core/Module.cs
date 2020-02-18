using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Baskid.Core;
using Baskid.Core.Provider;
using Microsoft.Extensions.CommandLineUtils;

namespace Baskid.Module.Core
{
    public class Module : ABaskidModule
    {
        private readonly IQueryStoreManager _manager;

        public Module(IQueryStoreManager manager)
        {
            _manager = manager;
            Commands = new Dictionary<string, Action<CommandLineApplication>>();
            Commands.Add("init", InitCommand);
            Commands.Add("status", StatusCommand);
            Commands.Add("add", AddCommand);
        }

        public override Dictionary<string, Action<CommandLineApplication>> Commands { get; set; }

        private  void InitCommand(CommandLineApplication command)
        {
            command.Description = "Initialize baskid repository";
            command.OnExecute(() =>
            {
                if (!Directory.Exists(BasikdDirectoryName))
                {
                    var di = Directory.CreateDirectory(BasikdDirectoryName);
                    di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;

                    using (var stream = File.Create(Path.Combine(BasikdDirectoryName, "config")))
                    {
                        var config = new BaskidRepositoryConfiguration();
                        config.DatabaseProvider = "sqlite";
                        var json = JsonSerializer.Serialize(config);
                        stream.Write(Encoding.Default.GetBytes(json));
                        stream.Flush();
                    }

                    Console.WriteLine($"initialized empty baskid repository in {BaskidDirectoryPath}");
                    return 0;
                }

                Console.WriteLine($"Reinitialized existing baskid repository in {Path.Combine(Directory.GetCurrentDirectory(), ".baskid")}");
                return 1;
            });
        }

        private  void StatusCommand(CommandLineApplication command)
        {
            command.Description = "Display status";
            command.OnExecute(() =>
            {
                Console.WriteLine("not a baskid repository");
                return 0;
            });
        }

        private  void AddCommand(CommandLineApplication command)
        {
            command.Description = "Add entry";
            command.OnExecute(() =>
            {
                _manager.Add(string.Join(" ", command.Arguments.Select(ca => ca.Value)));
                Console.WriteLine("not a baskid repository");
                return 0;
            });
        }
    }
}