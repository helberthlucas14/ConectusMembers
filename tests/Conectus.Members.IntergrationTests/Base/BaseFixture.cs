using Bogus;
using Conectus.Members.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace Conectus.Members.IntergrationTests.Base
{
    public class BaseFixture
    {
        public BaseFixture()
            => Faker = new Faker("pt_BR");

        protected Faker Faker { get; set; }

        public ConectusMemberDbContext CreateDbContext(
            bool preserveData = false,
            string? dbName = "")
        {
            var context = new ConectusMemberDbContext(
                new DbContextOptionsBuilder<ConectusMemberDbContext>()
                .UseInMemoryDatabase(string.IsNullOrWhiteSpace(dbName) ? "integration-tests-db" : dbName)
                .Options
            );
            if (!preserveData)
                context.Database.EnsureDeleted();
            return context;
        }
    }
}
