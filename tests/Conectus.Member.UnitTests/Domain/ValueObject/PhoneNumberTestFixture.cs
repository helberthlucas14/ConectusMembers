using Conectus.Members.Domain.ValueObject;
using Conectus.Members.UnitTests.Common;

namespace Conectus.Members.UnitTests.Domain.ValueObject
{
    public class PhoneNumberTestFixture : BaseFixture
    {
        public PhoneNumber GetValidPhoneNumber()
         => new PhoneNumber(Faker.Phone.PhoneNumber("(##)###-###-###"));
    }

    [CollectionDefinition(nameof(PhoneNumberTestFixture))]
    public class PhoneNumberTestFixtureCollection : ICollectionFixture<PhoneNumberTestFixture>
    {

    }
}
