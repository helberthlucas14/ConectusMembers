using Conectus.Members.Application.Exceptions;
using Conectus.Members.Infra.Data.EF.Repositories;
using FluentAssertions;
using UseCase = Conectus.Members.Application.UseCases.Member.GetMember;

namespace Conectus.Members.IntergrationTests.Application.UseCases.Member.GetMember
{
    [Collection(nameof(GetMemberTestFixture))]
    public class GetMemberTest
    {
        private readonly GetMemberTestFixture _fixture;
        public GetMemberTest(GetMemberTestFixture fixture) => _fixture = fixture;


        [Fact(DisplayName = nameof(GetAdultMember))]
        [Trait("Integration/Application", "GetMember - Use Cases")]
        public async Task GetAdultMember()
        {
            var exampleMember = _fixture.GetValidMemberExample();
            var dbContext = _fixture.CreateDbContext();
            dbContext.Members.Add(exampleMember);
            dbContext.SaveChanges();
            var repository = new MemberRepository(dbContext);

            var input = new UseCase.GetMemberInput(exampleMember.Id);
            var useCase = new UseCase.GetMember(repository);

            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Id.Should().NotBeEmpty();
            output.FirstName.Should().Be(exampleMember.FirstName);
            output.LastName.Should().Be(exampleMember.LastName);
            output.DateOfBirth.Should().Be(exampleMember.DateOfBirth);
            output.Gender.Should().Be(exampleMember.Gender);
            output.PhoneNumber.Should().Be(exampleMember.PhoneNumber.Value);
            output.Document.Should().NotBeNull();
            output.Document.Number.Should().Be(exampleMember.Document.Document);
            output.Document.Type.Should().Be(exampleMember.Document.Type);

            output.Address.Should().NotBeNull();
            output.Address.Street.Should().Be(exampleMember.Address.Street);
            output.Address.Number.Should().Be(exampleMember.Address.Number);
            output.Address.Complement.Should().Be(exampleMember.Address.Complement);
            output.Address.District.Should().Be(exampleMember.Address.District);
            output.Address.City.Should().Be(exampleMember.Address.City);
            output.Address.State.Should().Be(exampleMember.Address.State);
            output.Address.ZipCode.Should().Be(exampleMember.Address.ZipCode);
            output.Address.Latitude.Should().Be(exampleMember.Address.Latitude);
            output.Address.Longitude.Should().Be(exampleMember.Address.Longitude);
            output.IsMinor.Should().BeFalse();
            output.Responsible.Should().BeNull();
            output.ResponsibleId.Should().BeNull();
            output.IsActive.Should().Be(exampleMember.IsActive);
        }


        [Fact(DisplayName = nameof(GetMinorMember))]
        [Trait("Integration/Application", "GetMember - Use Cases")]
        public async Task GetMinorMember()
        {
            var exampleAdultMember = _fixture.GetValidMemberExample();
            var actContext = _fixture.CreateDbContext();
            actContext.Members.Add(exampleAdultMember);
            actContext.SaveChanges();

            var adultMember = actContext.Members.FirstOrDefault();

            var dbContext = _fixture.CreateDbContext(true);

            var exampleMinorMember = _fixture.GetValidMemberExample(
                isMinor: true,
                responsibleId: adultMember!.Id);

            dbContext.Members.Add(exampleMinorMember);
            dbContext.SaveChanges();

            var repository = new MemberRepository(dbContext);

            var input = new UseCase.GetMemberInput(exampleMinorMember.Id);
            var useCase = new UseCase.GetMember(repository);

            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Id.Should().NotBeEmpty();
            output.FirstName.Should().Be(exampleMinorMember.FirstName);
            output.LastName.Should().Be(exampleMinorMember.LastName);
            output.DateOfBirth.Should().Be(exampleMinorMember.DateOfBirth);
            output.Gender.Should().Be(exampleMinorMember.Gender);
            output.PhoneNumber.Should().Be(exampleMinorMember.PhoneNumber.Value);
            output.Document.Should().NotBeNull();
            output.Document.Number.Should().Be(exampleMinorMember.Document.Document);
            output.Document.Type.Should().Be(exampleMinorMember.Document.Type);

            output.Address.Should().NotBeNull();
            output.Address.Street.Should().Be(exampleMinorMember.Address.Street);
            output.Address.Number.Should().Be(exampleMinorMember.Address.Number);
            output.Address.Complement.Should().Be(exampleMinorMember.Address.Complement);
            output.Address.District.Should().Be(exampleMinorMember.Address.District);
            output.Address.City.Should().Be(exampleMinorMember.Address.City);
            output.Address.State.Should().Be(exampleMinorMember.Address.State);
            output.Address.ZipCode.Should().Be(exampleMinorMember.Address.ZipCode);
            output.Address.Latitude.Should().Be(exampleMinorMember.Address.Latitude);
            output.Address.Longitude.Should().Be(exampleMinorMember.Address.Longitude);
            output.IsMinor.Should().BeTrue();

            output.ResponsibleId.Should().NotBeNull();
            output.ResponsibleId.Should().Be(exampleMinorMember.ResponsibleId);

            output.Responsible.Should().NotBeNull();
            output.Responsible!.Id.Should().Be(adultMember.Id);

            output.IsActive.Should().Be(exampleMinorMember.IsActive);
        }

        [Fact(DisplayName = nameof(NotFoundExceptionWhenMemberDoesntExist))]
        [Trait("Integration/Application", "GetMember - Use Cases")]
        public async Task NotFoundExceptionWhenMemberDoesntExist()
        {
            var dbContext = _fixture.CreateDbContext();
            var repository = new MemberRepository(dbContext);
            var exampleGuid = Guid.NewGuid();

            var input = new UseCase.GetMemberInput(exampleGuid);
            var useCase = new UseCase.GetMember(repository);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Member '{input.Id}' not found.");
        }
    }
}
