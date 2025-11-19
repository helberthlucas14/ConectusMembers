using Conectus.Members.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;
using DomainEntity = Conectus.Members.Domain.Entity;

namespace Conectus.Members.EndToEndTests.Api.Member.Common
{
    public class MemberPersistence
    {
        private readonly ConectusMemberDbContext _context;
        public MemberPersistence(ConectusMemberDbContext dbContext) 
            => _context = dbContext;
        public async Task<DomainEntity.Member?> GetById(Guid id)
            => await _context
                .Members.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task InsertList(List<DomainEntity.Member> categories)
        {
            await _context.Members.AddRangeAsync(categories);
            await _context.SaveChangesAsync();
        }
    }
}
