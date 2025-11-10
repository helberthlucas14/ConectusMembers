using Conectus.Members.Domain.Exceptions;
using Conectus.Members.UnitTests.Domain.Entity;
using FluentAssertions;
using DomainEntity = Conectus.Members.Domain.Entity;
namespace Conectus.Member.UnitTests.Domain.Entity
{
    [Collection(nameof(MemberTestFixture))]
    public class MemberTest
    {
        private readonly MemberTestFixture _fixture;
        public MemberTest(MemberTestFixture fixture)
            => _fixture = fixture;


        [Fact(DisplayName = nameof(Instantiate_Member_Successfully))]
        [Trait("Domain", "Member - Aggregates")]
        public void Instantiate_Member_Successfully()
        {
            var validMember = _fixture.GetValidMember();
            var datetimeBefore = DateTime.Now;

            var member = new DomainEntity.Member(
                validMember.FirstName,
                validMember.LastName,
                validMember.DateOfBirth,
                validMember.Gender,
                validMember.PhoneNumber,
                validMember.Document,
                validMember.Address);

            var datetimeAfter = DateTime.Now.AddSeconds(1);

            member.Should().NotBeNull();
            member.Id.Should().NotBeEmpty();
            member.FirstName.Should().Be(validMember.FirstName);
            member.LastName.Should().Be(validMember.LastName);
            member.DateOfBirth.Should().Be(validMember.DateOfBirth);
            member.Gender.Should().Be(validMember.Gender);
            member.PhoneNumber.Should().Be(validMember.PhoneNumber);
            member.Document.Should().Be(validMember.Document);
            member.Address.Should().Be(validMember.Address);
            member.IsActive.Should().BeTrue();
            member.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
            (member.CreatedAt >= datetimeBefore).Should().BeTrue();
            (member.CreatedAt <= datetimeAfter).Should().BeTrue();
        }




        [Fact(DisplayName = nameof(Instantiate_Member_IsActive))]
        [Trait("Domain", "Member - Aggregates")]
        public void Instantiate_Member_IsActive()
        {
            var validMember = _fixture.GetValidMember();
            var datetimeBefore = DateTime.Now;
            var isActive = true;

            var member = new DomainEntity.Member(
                validMember.FirstName,
                validMember.LastName,
                validMember.DateOfBirth,
                validMember.Gender,
                validMember.PhoneNumber,
                validMember.Document,
                validMember.Address,
                isActive: isActive);

            var datetimeAfter = DateTime.Now.AddSeconds(1);

            member.Should().NotBeNull();
            member.Id.Should().NotBeEmpty();
            member.FirstName.Should().Be(validMember.FirstName);
            member.LastName.Should().Be(validMember.LastName);
            member.DateOfBirth.Should().Be(validMember.DateOfBirth);
            member.Gender.Should().Be(validMember.Gender);
            member.PhoneNumber.Should().Be(validMember.PhoneNumber);
            member.Document.Should().Be(validMember.Document);
            member.Address.Should().Be(validMember.Address);
            member.IsActive.Should().Be(isActive);
            member.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
            (member.CreatedAt >= datetimeBefore).Should().BeTrue();
            (member.CreatedAt <= datetimeAfter).Should().BeTrue();
        }


        [Theory(DisplayName = nameof(InstantiateErrorWhenFullNameIsEmpty))]
        [Trait("Domain", "Member - Aggregates")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void InstantiateErrorWhenFullNameIsEmpty(string firstName)
        {
            var validMember = _fixture.GetValidMember();

            Action action = () => new DomainEntity.Member(
                           firstName,
                           validMember.LastName,
                           validMember.DateOfBirth,
                           validMember.Gender,
                           validMember.PhoneNumber,
                           validMember.Document,
                           validMember.Address);

            action.Should().Throw<EntityValidationException>()
                .WithMessage("FirstName should not be empty or null");

        }

        [Theory(DisplayName = nameof(InstantiateErrorWhenLastNameIsEmpty))]
        [Trait("Domain", "Member - Aggregates")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void InstantiateErrorWhenLastNameIsEmpty(string lastName)
        {
            var validMember = _fixture.GetValidMember();

            Action action = () => new DomainEntity.Member(
                           validMember.FirstName,
                           lastName,
                           validMember.DateOfBirth,
                           validMember.Gender,
                           validMember.PhoneNumber,
                           validMember.Document,
                           validMember.Address);

            action.Should().Throw<EntityValidationException>()
                .WithMessage("LastName should not be empty or null");

        }

        [Theory(DisplayName = nameof(InstantiateErrorWhenDateOfBirthEmpty))]
        [Trait("Domain", "Member - Aggregates")]
        [InlineData("0001-01-01")]
        public void InstantiateErrorWhenDateOfBirthEmpty(DateTime dateOfBirth)
        {
            var validMember = _fixture.GetValidMember();

            Action action = () => new DomainEntity.Member(
                           validMember.FirstName,
                           validMember.LastName,
                           dateOfBirth,
                           validMember.Gender,
                           validMember.PhoneNumber,
                           validMember.Document,
                           validMember.Address);

            action.Should().Throw<EntityValidationException>()
                .WithMessage("DateOfBirth should not be null");
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenIsMinorWithouResponsibleId))]
        [Trait("Domain", "Member - Aggregates")]
        public void InstantiateErrorWhenIsMinorWithouResponsibleId()
        {
            var isMinor = true;
            var validMember = _fixture.GetValidMember(isMinor);

            Action action = () => new DomainEntity.Member(
                           validMember.FirstName,
                           validMember.LastName,
                           validMember.DateOfBirth,
                           validMember.Gender,
                           validMember.PhoneNumber,
                           validMember.Document,
                           validMember.Address);

            action.Should().Throw<EntityValidationException>()
                .WithMessage("Member is a minor and needs a guardian.");
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenFirstIsGreaterThan50Characters))]
        [Trait("Domain", "Member - Aggregates")]
        public void InstantiateErrorWhenDateOfBirthLaterThanToday()
        {
            var validMember = _fixture.GetValidMember();
            var invalidDateOfBirth = DateTime.Now.AddDays(1);

            Action action = () => new DomainEntity.Member(
                           validMember.FirstName,
                           validMember.LastName,
                           invalidDateOfBirth,
                           validMember.Gender,
                           validMember.PhoneNumber,
                           validMember.Document,
                           validMember.Address);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("DateOfBirth is invalid.");
        }

        #region FirstName Validations
        [Theory(DisplayName = nameof(InstantiateErrorWhenFirstNameIsLessThan3Characters))]
        [Trait("Domain", "Member - Aggregates")]
        [MemberData(nameof(GetFirstNamesWithLessThan3Characters), parameters: 10)]
        public void InstantiateErrorWhenFirstNameIsLessThan3Characters(string invalidName)
        {
            var validMember = _fixture.GetValidMember();

            Action action = () => new DomainEntity.Member(
                           invalidName,
                           validMember.LastName,
                           validMember.DateOfBirth,
                           validMember.Gender,
                           validMember.PhoneNumber,
                           validMember.Document,
                           validMember.Address);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("FirstName should be at least 3 characters long");
        }

        public static IEnumerable<object[]> GetFirstNamesWithLessThan3Characters(int numberOfTests = 6)
        {
            var fixture = new MemberTestFixture();
            for (int i = 0; i < numberOfTests; i++)
            {
                var isOdd = i % 2 == 1;
                yield return new object[] {
                fixture.GetValidFirstName()[..(isOdd ? 1 : 2)]
            };
            }
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenFirstIsGreaterThan50Characters))]
        [Trait("Domain", "Member - Aggregates")]
        public void InstantiateErrorWhenFirstIsGreaterThan50Characters()
        {
            var validMember = _fixture.GetValidMember();
            var invalidName = String.Join(null, Enumerable.Range(51, 100).Select(_ => "a").ToArray());

            Action action = () => new DomainEntity.Member(
                           invalidName,
                           validMember.LastName,
                           validMember.DateOfBirth,
                           validMember.Gender,
                           validMember.PhoneNumber,
                           validMember.Document,
                           validMember.Address);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("FirstName should be less or equal 50 characters long");
        }
        #endregion

        #region LastName Validations
        [Theory(DisplayName = nameof(InstantiateErrorWhenLastNameIsLessThan3Characters))]
        [Trait("Domain", "Member - Aggregates")]
        [MemberData(nameof(GetFirstNamesWithLessThan3Characters), parameters: 10)]
        public void InstantiateErrorWhenLastNameIsLessThan3Characters(string invalidLastName)
        {
            var validMember = _fixture.GetValidMember();

            Action action = () => new DomainEntity.Member(
                           validMember.FirstName,
                           invalidLastName,
                           validMember.DateOfBirth,
                           validMember.Gender,
                           validMember.PhoneNumber,
                           validMember.Document,
                           validMember.Address);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("LastName should be at least 3 characters long");
        }

        public static IEnumerable<object[]> GetLastNamesWithLessThan3Characters(int numberOfTests = 6)
        {
            var fixture = new MemberTestFixture();
            for (int i = 0; i < numberOfTests; i++)
            {
                var isOdd = i % 2 == 1;
                yield return new object[] {
                fixture.GetValidLastName()[..(isOdd ? 1 : 2)]
            };
            }
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenLastNameIsGreaterThan50Characters))]
        [Trait("Domain", "Member - Aggregates")]
        public void InstantiateErrorWhenLastNameIsGreaterThan50Characters()
        {
            var validMember = _fixture.GetValidMember();
            var invalidLastName = String.Join(null, Enumerable.Range(51, 100).Select(_ => "a").ToArray());

            Action action = () => new DomainEntity.Member(
                           validMember.FirstName,
                           invalidLastName,
                           validMember.DateOfBirth,
                           validMember.Gender,
                           validMember.PhoneNumber,
                           validMember.Document,
                           validMember.Address);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("LastName should be less or equal 50 characters long");
        }

        #endregion
    }
}
