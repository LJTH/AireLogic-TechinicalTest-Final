using Microsoft.EntityFrameworkCore;
using PANDA.Repository.Context;

namespace PANDA.Tests.Helpers
{
    public abstract class TestSetup
    {
        protected static PandaDbContext PandaDbContext { get; set; }

        [SetUp]
        public void Init()
        {
            PandaDbContext = InMemoryContext<PandaDbContext>();
        }

        private T InMemoryContext<T>() where T : DbContext
        {

            var options = new DbContextOptionsBuilder<T>()
                                    .UseInMemoryDatabase(databaseName: $"InMemory{typeof(T).Name}")
                                    .Options;
            T context = Activator.CreateInstance(typeof(T), options) as T;


            return context;
        }


        [TearDown]
        public void TearDown()
        {
            PandaDbContext.Database.EnsureDeleted();
            PandaDbContext.Dispose();
        }
    }
}
