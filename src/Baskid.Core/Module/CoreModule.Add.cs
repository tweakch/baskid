using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.CommandLineUtils;

namespace Baskid.Core.Module
{
    public partial class CoreModule
    {
        private static readonly string __filterDescription = "Filter pattern to match against names of files in the current directory. " +
                                                             "This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, " +
                                                             "but it doesn't support regular expressions.";

        protected void AddCommand(CommandLineApplication command)
        {
            command.HelpOption("-?|--help");
            var queryArgument = command.Argument("[query]", "The query to be added", config => { config.ShowInHelpText = true; }, true);

            var allOption = command.Option("-a|--all", "add all files in the current directory", CommandOptionType.NoValue);
            var dirOption = command.Option("-d|--dir", "add directories only", CommandOptionType.NoValue);
            var showOption = command.Option("-s|--show", "shows the query result without adding it", CommandOptionType.NoValue);
            var filterOption = command.Option("-f|--filter", __filterDescription, CommandOptionType.SingleValue);
            var recursiveOption = command.Option("-r|--recursive", "include subdirectories", CommandOptionType.NoValue);

            command.OnExecute(() =>
            {
                if (!IsBaskidRepository) throw new InvalidOperationException("Not a baskid repository");

                IEnumerable<string> values;
                // if not --all
                if (allOption.HasValue() && !queryArgument.Values.Any())
                {
                    values = AddAll(filterOption, dirOption.HasValue(), recursiveOption.HasValue());
                }
                else
                {
                    values = queryArgument.Values;
                }

                // don't add values if the --show option is given
                if (showOption.HasValue())
                {
                    Console.WriteLine($"{values.ToJson()}");
                }
                else
                {
                    var id = _manager.Add(values);
                    Console.WriteLine($"Added query {id} => {values.ToJson()}");
                }

                return 0;
            });
        }

        private static IEnumerable<string> AddAll(CommandOption filterOption, bool dir, bool rec)
        {
            var pattern = "*";
            if (filterOption.HasValue()) pattern = filterOption.Value();
            var option = rec ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            var directory = CurrentDirectory;
            if (dir) 
                return Directory.EnumerateDirectories(directory, pattern, option);
            return Directory.EnumerateFiles(directory, pattern, option);
        }
    }
}