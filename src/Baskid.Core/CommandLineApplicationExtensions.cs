using System;
using Microsoft.Extensions.CommandLineUtils;

namespace Baskid.Core
{
    public static class CommandLineApplicationExtensions
    {
        public static void AddModule<TModule>(this CommandLineApplication app) where TModule : ABaskidModule
        {
            var module = Activator.CreateInstance<TModule>();
            foreach (var command in module.Commands)
            {
                app.Command(command.Key, command.Value);
            }
        }
    }
}