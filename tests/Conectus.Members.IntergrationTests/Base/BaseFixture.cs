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

        public ConectusMemberDbContext CreateDbContext(bool preserveData = false)
        {
            var context = new ConectusMemberDbContext(
                new DbContextOptionsBuilder<ConectusMemberDbContext>()
                .UseInMemoryDatabase("integration-tests-db")
                .Options
            );
            if (preserveData == false)
                context.Database.EnsureDeleted();
            return context;
        }
    }
}
