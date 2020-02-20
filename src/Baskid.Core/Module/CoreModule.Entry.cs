using System;
using System.Collections.Generic;
using System.Linq;
using Baskid.Core.Models;
using Microsoft.Extensions.CommandLineUtils;

namespace Baskid.Core.Module
{
    public partial class CoreModule
    {
        protected void EntryCommand(CommandLineApplication command)
        {
            command.Description = "Perform actions on entries";
            command.OnExecute(() =>
            {
                if (!IsBaskidRepository) throw new InvalidOperationException("Not a baskid repository");

                var context = _manager.Context;
                var existing = new HashSet<string>(context.SearchEntries.Select(s => s.Id));

                foreach (var searchEntry in context.SearchEntries)
                {
                    var separator = new[] {':', '\\'};
                    foreach (var value in searchEntry.Value.Split(separator, StringSplitOptions.RemoveEmptyEntries))
                    {
                        var hash = value.ToHash();
                        if (existing.Contains(hash))
                        {
                            _manager.Reference(searchEntry, hash);
                            continue;
                        }
                        
                        context.SearchEntries.Add(new Entry {Id = hash, Value = value});
                        context.SaveChanges();
                        existing.Add(hash);
                    }
                }

                var statusJson = new {queries = context.SearchQueries.Count(), entries = context.SearchEntries.Count()}.ToJson();
                Console.WriteLine(statusJson);
                return 0;
            });
        }
    }
}