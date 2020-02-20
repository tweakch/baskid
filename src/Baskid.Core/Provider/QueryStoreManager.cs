// The code in this solution is awesome!

using System;
using System.Collections.Generic;
using System.Linq;
using Baskid.Core.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
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

        public BaskidContext Context
        {
            get { return _context; }
        }

        public string HashName { get; set; }

        public ContextAction SaveChangesOn { get; set; }

        public Guid Add(string searchString)
        {
            var guid = Guid.NewGuid();
            Add(guid, searchString);
            return guid;
        }

        public Guid Add(IEnumerable<string> searchString)
        {
            var guid = Guid.NewGuid();
            Add(guid, searchString);
            return guid;
        }

        public void Add(Guid id, IEnumerable<string> searchString)
        {
            var query = new Query { Id = id, SearchString = null };
            var queryEntries = new List<QueryEntry>();

            foreach (var value in searchString.Distinct())
            {
                var hash = value.ToHash(HashName);

                var entry = Context.SearchEntries.Find(hash);
                if (entry == null)
                {
                    entry = new Entry { Id = hash, Value = value };
                    _context.SearchEntries.Add(entry);
                }

                var queryEntry = new QueryEntry { Entry = entry, Query = query };
                queryEntries.Add(queryEntry);
            }

            query.Entries = queryEntries;
            _context.SearchQueries.Add(query);
            if (IsSet(ContextAction.Add)) _context.SaveChanges();
        }

        public void Add(Guid id, string searchString)
        {
            var query = new Query {Id = id, SearchString = searchString};
            var queryEntries = new List<QueryEntry>();
            
            var entries = new List<string>() { searchString };
            entries.AddRange(searchString.Split(" "));
            
            foreach (var value in entries.Distinct())
            {
                var hash = value.ToHash(HashName);

                var entry = Context.SearchEntries.Find(hash);
                if (entry == null)
                {
                    entry = new Entry {Id = hash, Value = value};
                    _context.SearchEntries.Add(entry);
                }

                var queryEntry = new QueryEntry { Entry = entry, Query = query };
                queryEntries.Add(queryEntry);
            }

            query.Entries = queryEntries;
            _context.SearchQueries.Add(query);
            if (IsSet(ContextAction.Add)) _context.SaveChanges();
        }

        public void Reference(Entry entry, string hash)
        {
            _context.EntryReferences.Add(new EntryReference() { });
        }

        public IEnumerable<Entry> Entries
        {
            get
            {
                return _context.SearchEntries;
            }
        }

        private bool IsSet(ContextAction contextAction)
        {
            return (SaveChangesOn & contextAction) == contextAction;
        }
    }
}