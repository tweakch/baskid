using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Baskid.Core.Models;
using Baskid.Core.Provider;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Baskid.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<BaskidContext>().UseSqlite(connection).Options;

            using (var context = new BaskidContext(options))
            {
                context.Database.EnsureCreated();
            }

            var guid1 = Guid.NewGuid();
            using (var context = new BaskidContext(options))
            {
                var queryManager = new QueryStoreManager(context) { HashName = "MD5", SaveChangesOn = ContextAction.Add | ContextAction.Remove };
                queryManager.Add(guid1, "V13432");
            }

            using (var context = new BaskidContext(options))
            {
                var provider = new QueryProvider(context);
                var query = await provider.GetAsync(guid1);

                Assert.AreEqual("V13432", query.SearchString);
                Assert.AreEqual("[\"cb746ffef1809dbb4b71b08a6088acc7\"]", JsonSerializer.Serialize(query.Entries.Select(s => s.EntryId)));
                Assert.AreEqual("V13432", query.Entries.First().Entry.Value);
            }
        }

        [TestMethod]
        public async Task TestMethod2()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<BaskidContext>().UseSqlite(connection).Options;

            using (var context = new BaskidContext(options))
            {
                context.Database.EnsureCreated();
            }

            var guid1 = Guid.NewGuid();
            using (var context = new BaskidContext(options))
            {
                var queryManager = new QueryStoreManager(context) { HashName = "MD5", SaveChangesOn = ContextAction.Add | ContextAction.Remove };
                queryManager.Add(guid1, "V13432 V12345");
            }

            using (var context = new BaskidContext(options))
            {
                var provider = new QueryProvider(context);
                var query = await provider.GetAsync(guid1);

                Assert.AreEqual("V13432 V12345", query.SearchString);
                Assert.AreEqual("[\"11c4d5c85da3bf3075e762c4c124b04f\",\"a3f3960ad4aacda10b41fe63cc725375\",\"cb746ffef1809dbb4b71b08a6088acc7\"]", JsonSerializer.Serialize(query.Entries.Select(s => s.EntryId)));
                Assert.AreEqual("V12345", query.Entries.First().Entry.Value);
            }
        }
    }
}