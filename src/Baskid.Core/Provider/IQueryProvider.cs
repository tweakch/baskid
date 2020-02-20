using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Baskid.Core.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Baskid.Core.Provider
{
    public interface IQueryProvider
    {
        Task<Query> GetAsync(Guid id);
    }
    public interface IQueryStoreManager
    {
        public BaskidContext Context { get; }
        public string HashName { get; set; }
        ContextAction SaveChangesOn {get;set;}

        Guid Add(string searchString);
        Guid Add(IEnumerable<string> searchString);

        /// <summary>
        /// Add a new query
        /// </summary>
        /// <param name="id"></param>
        /// <param name="searchString"></param>
        void Add(Guid id, string searchString);

        /// <summary>
        /// Add an entry reference
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="hash"></param>
        void Reference(Entry entry, string hash);
    }

    [Flags]
    public enum ContextAction
    {
        Add = 1, Remove = 2, Update = 4
    }

}