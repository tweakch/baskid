using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.CommandLineUtils;

namespace Baskid.Core
{
    public abstract class ABaskidModule
    {
        public static string BasikdDirectoryName = ".baskid";

        public static string BaskidDirectoryPath
        {
            get { return Path.Combine(CurrentDirectory, BasikdDirectoryName); }
        }

        public static string CurrentDirectory
        {
            get { return Directory.GetCurrentDirectory(); }
        }

        public static bool IsBaskidRepository
        {
            get { return Directory.Exists(BaskidDirectoryPath); }
        }

        public abstract Dictionary<string, Action<CommandLineApplication>> Commands { get; set; }
    }
}