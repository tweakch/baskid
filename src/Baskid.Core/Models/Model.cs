using System;
using System.Collections.Generic;

namespace Baskid.Core.Models
{
    public class Query
    {
        public Guid Id { get; set; }
        public string SearchString { get; set; }
        public virtual ICollection<QueryEntry> Entries { get; set; }
    }
    
    public class Entry
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public virtual ICollection<QueryEntry> Queries { get; set; }
    }

    public class QueryEntry
    {
        public Guid QueryId { get; set; }
        public Query Query { get; set; }

        public string EntryId { get; set; }
        public Entry Entry { get; set; }
    }
}