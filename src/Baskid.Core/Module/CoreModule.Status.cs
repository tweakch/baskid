using System;
using System.Linq;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace Baskid.Core.Module
{
    public partial class CoreModule
    {
        protected void StatusCommand(CommandLineApplication command)
        {
            command.Description = "Display status";
            command.OnExecute(() =>
            {
                if (!IsBaskidRepository)
                {
                    throw new InvalidOperationException("Not a baskid repository");
                }

                var statusJson = new {queries = _manager.Context.SearchQueries.Count(), entries = _manager.Context.SearchEntries.Count()}.ToJson();
                Console.WriteLine(statusJson);
                return 0;
            });
        }
    }
}