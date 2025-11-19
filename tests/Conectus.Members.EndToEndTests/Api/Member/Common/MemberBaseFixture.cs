using Conectus.Members.EndToEndTests.Base;

namespace Conectus.Members.EndToEndTests.Api.Member.Common
{
    public class MemberBaseFixture 
        : BaseFixture
    {
        public MemberPersistence Persistence;

        public MemberBaseFixture()
        {
            Persistence = new MemberPersistence(
                CreateDbContext()
            );
        }
    }
}
