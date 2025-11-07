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
                isActive);

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
    }
}
