using System;
using System.Threading.Tasks;
using Baskid.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Baskid.Core.Provider
{
    public class QueryProvider : IQueryProvider
    {
        private readonly BaskidContext _context;

        public QueryProvider(BaskidContext context)
        {
            _context = context;
        }

        public async Task<Query> GetAsync(Guid id)
        {
            return await _context.SearchQueries.Include(s => s.Entries)
                                 .ThenInclude(sq => sq.Entry)
                                 .ThenInclude(e => e.Queries)
                                 .FirstOrDefaultAsync(q => q.Id == id);
        }
    }
}