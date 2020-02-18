using Microsoft.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.IO;

namespace Baskid.Core
{
    public abstract class ABaskidModule
    {
        public static string BaskidDirectoryPath { get{return Path.Combine(Directory.GetCurrentDirectory(), BasikdDirectoryName); } }
        public static bool IsBaskidRepository { get { return Directory.Exists(BaskidDirectoryPath); } }
        public static string BasikdDirectoryName = ".baskid";
        public abstract Dictionary<string, Action<CommandLineApplication>> Commands { get; set; }
    }
}