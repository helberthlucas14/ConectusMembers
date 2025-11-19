using Conectus.Members.Application;
using Conectus.Members.Application.UseCases.Member.CreateMember;
using Conectus.Members.Domain.Exceptions;
using Conectus.Members.Infra.Data.EF;
using Conectus.Members.Infra.Data.EF.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UseCase = Conectus.Members.Application.UseCases.Member.CreateMember;

namespace Conectus.Members.IntergrationTests.Application.UseCases.Member.CreateMember
{
    [Collection(nameof(CreateMemberTestFixture))]
    public class CreateMemberTests
    {
        private readonly CreateMemberTestFixture _fixture;
        public CreateMemberTests(CreateMemberTestFixture fixture) => _fixture = fixture;

        [Fact(DisplayName = nameof(CreateAdultMemberSuccessfully))]
        [Trait("Integration/Application", "CreateMember - Use Cases")]
        public async Task CreateAdultMemberSuccessfully()
        {
            var actContext = _fixture.CreateDbContext();
            var respository = new MemberRepository(actContext);
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var eventPublisher = new DomainEventPublisher(serviceProvider);
            var unitOfWork = new UnitOfWork(
                actContext,
                eventPublisher,
                serviceProvider.GetRequiredService<ILogger<UnitOfWork>>());

            var useCase = new UseCase.CreateMember(
                respository,
                unitOfWork);

            var input = _fixture.GetExampleInput();

            var output = await useCase.Handle(input, CancellationToken.None);
            output.Should().NotBeNull();
            output.Id.Should().NotBeEmpty();
            output.FirstName.Should().Be(input.FirstName);
            output.LastName.Should().Be(input.LastName);
            output.DateOfBirth.Should().Be(input.DateOfBirth);
            output.Gender.Should().Be(input.Gender);
            output.PhoneNumber.Should().Be(input.PhoneNumber);
            output.Document.Should().Be(input.Document);
            output.Address.Should().NotBeNull();
            output.Address.Street.Should().Be(input.Address.Street);
            output.Address.Number.Should().Be(input.Address.Number);
            output.Address.Complement.Should().Be(input.Address.Complement);
            output.Address.District.Should().Be(input.Address.District);
            output.Address.City.Should().Be(input.Address.City);
            output.Address.State.Should().Be(input.Address.State);
            output.Address.ZipCode.Should().Be(input.Address.ZipCode);
            output.Address.Latitude.Should().Be(input.Address.Latitude);
            output.Address.Longitude.Should().Be(input.Address.Longitude);
            output.IsActive.Should().BeTrue();
            output.ResponsibleId.Should().BeNull();
            output.Responsible.Should().BeNull();
            output.CreatedAt.Should().NotBe(default);

            var assertContext = _fixture.CreateDbContext(true);

            var members = await assertContext.Members.AsNoTracking().ToListAsync();
            members.Should().HaveCount(1);
            var member = members[0];
            member.Id.Should().Be(output.Id);
            member.Id.Should().NotBeEmpty();
            member.FirstName.Should().Be(input.FirstName);
            member.LastName.Should().Be(input.LastName);
            member.DateOfBirth.Should().Be(input.DateOfBirth);
            member.Gender.Should().Be(input.Gender);
            member.PhoneNumber.Value.Should().Be(input.PhoneNumber);
            member.Document.Should().NotBeNull();
            member.Document.Document.Should().Be(input.Document.Number);
            member.Document.Type.Should().Be(input.Document.Type);
            member.Address.Should().NotBeNull();
            member.Address.Street.Should().Be(input.Address.Street);
            member.Address.Number.Should().Be(input.Address.Number);
            member.Address.Complement.Should().Be(input.Address.Complement);
            member.Address.District.Should().Be(input.Address.District);
            member.Address.City.Should().Be(input.Address.City);
            member.Address.State.Should().Be(input.Address.State);
            member.Address.ZipCode.Should().Be(input.Address.ZipCode);
            member.Address.Latitude.Should().Be(input.Address.Latitude);
            member.Address.Longitude.Should().Be(input.Address.Longitude);
            member.ResponsibleId.Should().BeNull();
            member.Responsible.Should().BeNull();
            member.IsActive.Should().BeTrue();
        }


        [Fact(DisplayName = nameof(CreateMinorMemberSuccessfully))]
        [Trait("Integration/Application", "CreateMember - Use Cases")]
        public async Task CreateMinorMemberSuccessfully()
        {
            var actContext = _fixture.CreateDbContext();
            var respository = new MemberRepository(actContext);
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var eventPublisher = new DomainEventPublisher(serviceProvider);
            var unitOfWork = new UnitOfWork(
                actContext,
                eventPublisher,
                serviceProvider.GetRequiredService<ILogger<UnitOfWork>>());

            var useCaseAdult = new UseCase.CreateMember(
                respository,
                unitOfWork);


            var inputAdult = _fixture.GetExampleInput();

            var adultOutput = await useCaseAdult.Handle(inputAdult, CancellationToken.None);

            var inputMinor = _fixture.GetExampleInput(isMinor: true, adultOutput.Id);

            var output = await useCaseAdult.Handle(inputMinor, CancellationToken.None);

            output.Should().NotBeNull();
            output.Id.Should().NotBeEmpty();
            output.FirstName.Should().Be(inputMinor.FirstName);
            output.LastName.Should().Be(inputMinor.LastName);
            output.DateOfBirth.Should().Be(inputMinor.DateOfBirth);
            output.Gender.Should().Be(inputMinor.Gender);
            output.PhoneNumber.Should().Be(inputMinor.PhoneNumber);
            output.Document.Should().Be(inputMinor.Document);
            output.Address.Should().NotBeNull();
            output.Address.Street.Should().Be(inputMinor.Address.Street);
            output.Address.Number.Should().Be(inputMinor.Address.Number);
            output.Address.Complement.Should().Be(inputMinor.Address.Complement);
            output.Address.District.Should().Be(inputMinor.Address.District);
            output.Address.City.Should().Be(inputMinor.Address.City);
            output.Address.State.Should().Be(inputMinor.Address.State);
            output.Address.ZipCode.Should().Be(inputMinor.Address.ZipCode);
            output.Address.Latitude.Should().Be(inputMinor.Address.Latitude);
            output.Address.Longitude.Should().Be(inputMinor.Address.Longitude);
            output.IsActive.Should().BeTrue();
            output.ResponsibleId.Should().Be(inputMinor.ResponsibleId);
            output.Responsible.Should().NotBeNull();
            output.CreatedAt.Should().NotBe(default);

            var assertContext = _fixture.CreateDbContext(true);
            var members = await assertContext.Members
                .Include(r => r.Responsible)
                .AsNoTracking()
                .ToListAsync();

            members.Should().HaveCount(2);

            var adultMember = members[0];
            var minorMember = members[1];
            minorMember.Id.Should().Be(output.Id);
            minorMember.Id.Should().NotBeEmpty();
            minorMember.FirstName.Should().Be(inputMinor.FirstName);
            minorMember.LastName.Should().Be(inputMinor.LastName);
            minorMember.DateOfBirth.Should().Be(inputMinor.DateOfBirth);
            minorMember.Gender.Should().Be(inputMinor.Gender);
            minorMember.PhoneNumber.Value.Should().Be(inputMinor.PhoneNumber);
            minorMember.Document.Should().NotBeNull();
            minorMember.Document.Document.Should().Be(inputMinor.Document.Number);
            minorMember.Document.Type.Should().Be(inputMinor.Document.Type);
            minorMember.Address.Should().NotBeNull();
            minorMember.Address.Street.Should().Be(inputMinor.Address.Street);
            minorMember.Address.Number.Should().Be(inputMinor.Address.Number);
            minorMember.Address.Complement.Should().Be(inputMinor.Address.Complement);
            minorMember.Address.District.Should().Be(inputMinor.Address.District);
            minorMember.Address.City.Should().Be(inputMinor.Address.City);
            minorMember.Address.State.Should().Be(inputMinor.Address.State);
            minorMember.Address.ZipCode.Should().Be(inputMinor.Address.ZipCode);
            minorMember.Address.Latitude.Should().Be(inputMinor.Address.Latitude);
            minorMember.Address.Longitude.Should().Be(inputMinor.Address.Longitude);
            minorMember.ResponsibleId.Should().NotBeNull();
            minorMember.ResponsibleId.Should().Be(adultMember.Id);
            minorMember.Responsible.Should().NotBeNull();
            minorMember.Responsible.Id.Should().Be(adultMember.Id);
            minorMember.Responsible.Document.Document.Should().Be(adultMember.Document.Document);
            minorMember.IsActive.Should().BeTrue();
        }



        [Fact(DisplayName = nameof(ThrowWhenMinorWithoutResposibleId))]
        [Trait("Integration/Application", "CreateMember - Use Cases")]
        public async Task ThrowWhenMinorWithoutResposibleId()
        {
            var isMinorInput = true;
            var input = _fixture.GetExampleInput(isMinorInput);

            var actContext = _fixture.CreateDbContext();
            var respository = new MemberRepository(actContext);
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var eventPublisher = new DomainEventPublisher(serviceProvider);
            var unitOfWork = new UnitOfWork(
                actContext,
                eventPublisher,
                serviceProvider.GetRequiredService<ILogger<UnitOfWork>>());

            var useCase = new UseCase.CreateMember(
                respository,
                unitOfWork);

            var action = async () => await useCase.Handle(
                   input,
                   CancellationToken.None
                   );

            await action.Should()
                .ThrowAsync<EntityValidationException>()
                .WithMessage("Member is a minor and needs a guardian.");
        }

        [Theory(DisplayName = nameof(ThrowWhenCantInstantiateMember))]
        [Trait("Integration/Application", "CreateCategory - Use Cases")]
        [MemberData(
                 nameof(CreateMemberTestDataGenerator.GetInvalidCreateMemberInputs),
                 parameters: 50,
                  MemberType = typeof(CreateMemberTestDataGenerator)
            )]
        public async void ThrowWhenCantInstantiateMember(
        CreateMemberInput input,
        string exceptionMessage
    )
        {
            var actContext = _fixture.CreateDbContext();
            var respository = new MemberRepository(actContext);
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var eventPublisher = new DomainEventPublisher(serviceProvider);
            var unitOfWork = new UnitOfWork(
                actContext,
                eventPublisher,
                serviceProvider.GetRequiredService<ILogger<UnitOfWork>>());

            var useCase = new UseCase.CreateMember(
                respository,
                unitOfWork
                );

            Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should()
                .ThrowAsync<EntityValidationException>()
                .WithMessage(exceptionMessage);
        }

    }
}
