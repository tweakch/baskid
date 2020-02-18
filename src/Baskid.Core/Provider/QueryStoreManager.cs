// The code in this solution is awesome!

using System;
using System.Collections.Generic;
using System.Linq;
using Baskid.Core.Models;
using Microsoft.EntityFrameworkCore.Internal;

namespace Baskid.Core.Provider
{
    public class QueryStoreManager : IQueryStoreManager
    {
        private readonly BaskidContext _context;
        
        public QueryStoreManager(BaskidContext context)
        {
            HashName = "MD5";
            _context = context;
        }

        public string HashName { get; set; }

        public ContextAction SaveChangesOn { get; set; }

        public Guid Add(string searchString)
        {
            var guid = Guid.NewGuid();
            Add(guid, searchString);
            return guid;
        }

        public void Add(Guid id, string searchString)
        {
            var query = new Query {Id = id, SearchString = searchString};
            var queryEntries = new List<QueryEntry>();
            
            var entries = new List<string>() { searchString };
            entries.AddRange(searchString.Split(" "));
            
            foreach (var value in entries.Distinct())
            {
                var entry = AddQueryEntry(value, query, queryEntries);
                _context.SearchEntries.Add(entry);
            }

            query.Entries = queryEntries;
            
            _context.SearchQueries.Add(query);
            if (IsSet(ContextAction.Add)) _context.SaveChanges();
        }

        private Entry AddQueryEntry(string searchString, Query query, List<QueryEntry> queryEntries)
        {
            var entry = new Entry {Id = searchString.ToHash(HashName), Value = searchString};
            var queryEntry = new QueryEntry {Entry = entry, Query = query};
            queryEntries.Add(queryEntry);
            return entry;
        }

        private bool IsSet(ContextAction contextAction)
        {
            return (SaveChangesOn & contextAction) == contextAction;
        }
    }
}