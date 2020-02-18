using System;
using System.IO;
using System.Text.Json;

using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.CommandLineUtils;
using System.Text;
using Baskid.Core;
using Baskid.Core.Provider;

namespace Baskid
{
    internal class BaskidCore : ABaskidModule
    {
        private readonly IQueryStoreManager _manager;

        public BaskidCore(IQueryStoreManager manager)
        {
            _manager = manager;

            Commands = new Dictionary<string, Action<CommandLineApplication>>();
            Commands.Add("init", InitCommand);
            Commands.Add("status", StatusCommand);   
            Commands.Add("add", AddCommand);
        }

        void InitCommand(CommandLineApplication command)
        {
            command.Description = "Initialize baskid repository";
            command.OnExecute(() =>
            {
                if (!Directory.Exists(BasikdDirectoryName))
                {
                    var di = Directory.CreateDirectory(BasikdDirectoryName);
                    di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;

                    using(var stream = File.Create(Path.Combine(BasikdDirectoryName, "config"))) 
                    {
                        BaskidRepositoryConfiguration config = new BaskidRepositoryConfiguration();      
                        config.DatabaseProvider = "sqlite";
                        var json = JsonSerializer.Serialize(config);
                        stream.Write(Encoding.Default.GetBytes(json));
                        stream.Flush();
                    }
                    
                    Console.WriteLine($"initialized empty baskid repository in {BaskidDirectoryPath}");
                    return 0;
                }
                else
                {
                    Console.WriteLine($"Reinitialized existing baskid repository in {Path.Combine(Directory.GetCurrentDirectory(), ".baskid")}");
                    return 1;
                }
            });
        }

        void StatusCommand(CommandLineApplication command)
        {
            command.Description = "Display status";
            command.OnExecute(() =>
            {
                Console.WriteLine("not a baskid repository");
                return 0;
            });
        }

        void AddCommand(CommandLineApplication command)
        {
            command.Description = "Add entry";
            command.OnExecute(() =>
            {
                _manager.Add(string.Join(" ", command.Arguments.Select(ca => ca.Value)));
                return 0;
            });
        }
        public override Dictionary<string, Action<CommandLineApplication>> Commands { get; set; }
    }
}