using System;
using System.Threading.Tasks;
using Baskid.Core.Models;

namespace Baskid.Core.Provider
{
    public interface IQueryProvider
    {
        Task<Query> GetAsync(Guid id);
    }
    public interface IQueryStoreManager
    {
        public string HashName { get; set; }
        ContextAction SaveChangesOn {get;set;} 
        Guid Add(string searchString);
        void Add(Guid id, string searchString);
    }

    [Flags]
    public enum ContextAction
    {
        Add = 1, Remove = 2, Update = 4
    }

}