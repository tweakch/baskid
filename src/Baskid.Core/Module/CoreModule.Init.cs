using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace Baskid.Core.Module
{
    public partial class CoreModule
    {
        protected void InitCommand(CommandLineApplication command)
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

                    _manager.Context.Database.EnsureCreated();
                    Console.WriteLine($"Initialized empty baskid repository in {Path.Combine(Directory.GetCurrentDirectory(), ".baskid")}");
                }
                else
                {
                    _manager.Context.Database.EnsureCreated();
                    Console.WriteLine($"Reinitialized existing baskid repository in {Path.Combine(Directory.GetCurrentDirectory(), ".baskid")}");
                }

                return 0;
            });
        }
    }
}
