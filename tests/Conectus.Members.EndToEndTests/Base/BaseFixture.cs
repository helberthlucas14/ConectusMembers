using Bogus;
using Conectus.Members.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Conectus.Members.EndToEndTests.Base
{
    public class BaseFixture : IDisposable
    {
        protected Faker Faker { get; set; }
        public CustomWebApplicationFactory<Program> WebAppFactory { get; set; }
        public HttpClient HttpClient { get; set; }
        public ApiClient ApiClient { get; set; }

        private readonly string _dbConnectionString;
        public BaseFixture()
        {
            Faker = new Faker("pt_BR");
            WebAppFactory = new CustomWebApplicationFactory<Program>();
            HttpClient = WebAppFactory.CreateClient();

            var configuration = WebAppFactory.Services
                .GetRequiredService<IConfiguration>();

            ApiClient = new ApiClient(HttpClient);
            ArgumentNullException.ThrowIfNull(configuration);
            _dbConnectionString = configuration.GetConnectionString("CatalogDb")!;
        }

        public ConectusMemberDbContext CreateDbContext()
        {
            var context = new ConectusMemberDbContext(
                new DbContextOptionsBuilder<ConectusMemberDbContext>()
                .UseSqlServer(_dbConnectionString)
                .Options
            );
            return context;
        }

        public void CleanPersistence()
        {
            var context = CreateDbContext();
            context.Database.EnsureDeleted();
        }

        public void Dispose()
        {
            WebAppFactory.Dispose();
        }
    }
}
