using System;
using System.Collections.Generic;
using Baskid.Core.Provider;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace Baskid.Core.Module
{
    public partial class CoreModule : ABaskidModule
    {
        protected readonly ILogger<CoreModule> _logger;
        private readonly IQueryStoreManager _manager;

        public CoreModule(ILogger<CoreModule> logger, IQueryStoreManager manager)
        {
            _logger = logger;
            _manager = manager;
            _manager.SaveChangesOn = ContextAction.Add;

            Commands = new Dictionary<string, Action<CommandLineApplication>>
            {
                {"init", InitCommand}, {"status", StatusCommand}, {"add", AddCommand}, {"build", BuildCommand}
            };
        }

        public override Dictionary<string, Action<CommandLineApplication>> Commands { get; set; }
    }
}