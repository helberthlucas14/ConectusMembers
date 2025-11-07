using Bogus;
using Conectus.Members.Domain.ValueObject;

namespace Conectus.Members.UnitTests.Common
{
    public abstract class BaseFixture
    {
        public Faker Faker { get; set; }

        protected BaseFixture()
            => Faker = new Faker("pt_BR");

        public bool GetRandomBoolean()
            => new Random().NextDouble() < 0.5;

        public Address GetAddress()
        {
            var number = Faker.Random.Long(1, 9_999_999_999).ToString();
            return new Address(
                Faker.Address.StreetName(),
                number,
                Faker.Address.SecondaryAddress(),
                Faker.Address.County(),
                Faker.Address.City(),
                Faker.Address.StateAbbr(),
                Faker.Address.ZipCode(),
                Faker.Random.Number(-90, 90),
                Faker.Random.Number(-180, 180));
        }
    }
}
