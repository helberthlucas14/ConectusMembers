using Conectus.Members.Domain.Entity;
using Conectus.Members.Infra.Data.EF.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Conectus.Members.Infra.Data.EF
{
    public class ConectusMemberDbContext
        : DbContext
    {
        public DbSet<Member> Members => Set<Member>();

        public ConectusMemberDbContext(
        DbContextOptions<ConectusMemberDbContext> options
        ) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new MemberConfiguration());
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            var entries = ChangeTracker
                .Entries<Member>()
                .Where(e => e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                entry.Property("LastUpdated").CurrentValue = DateTime.Now;
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
