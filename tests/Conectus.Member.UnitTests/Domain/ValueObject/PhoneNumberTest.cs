using Conectus.Members.Domain.Enum;
using Conectus.Members.Domain.ValueObject;
using FluentAssertions;

namespace Conectus.Members.UnitTests.Domain.ValueObject
{

    [Collection(nameof(PhoneNumberTestFixture))]
    public class PhoneNumberTest
    {
        private readonly PhoneNumberTestFixture _fixture;
        public PhoneNumberTest(PhoneNumberTestFixture fixture) => _fixture = fixture;

        [Fact(DisplayName = nameof(InstantiatePhoneNumberSuccessfully))]
        [Trait("Domain", "PhomeNumber - ValueObject")]
        public void InstantiatePhoneNumberSuccessfully()
        {
            var validPhoneNumber = _fixture.GetValidPhoneNumber();

            var phoneNumber = new PhoneNumber(validPhoneNumber.Value);

            phoneNumber.Should().NotBeNull();
            phoneNumber.Value.Should().Be(validPhoneNumber.Value);
        }

        [Theory(DisplayName = nameof(InstantiatePhoneNumberSuccessfully))]
        [Trait("Domain", "PhomeNumber - ValueObject")]
        [MemberData(nameof(GetInvalidNumbers), parameters: 10)]
        public void InstantiateErrorWhenPhoneNumberInvalidNumber(string number)
        {
            Action action = () => new PhoneNumber(number);

            action.Should().Throw()
                .WithMessage("PhoneNumber is invalid.");

        }

        public static IEnumerable<object[]> GetInvalidNumbers(int numberOfTests)
        {
            var fixture = new PhoneNumberTestFixture();

            for (int i = 0; i < numberOfTests; i++)
            {
                var isOdd = i % 2 == 1;
                yield return new object[] {
                fixture.GetValidPhoneNumber().Value[..(isOdd ? 11 : 13)]
            };
            }
        }

    }
}
