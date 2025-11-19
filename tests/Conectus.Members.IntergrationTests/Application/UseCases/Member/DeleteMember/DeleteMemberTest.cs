using Conectus.Members.Application;
using Conectus.Members.Application.Exceptions;
using Conectus.Members.Infra.Data.EF;
using Conectus.Members.Infra.Data.EF.Repositories;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UseCase = Conectus.Members.Application.UseCases.Member.DeleteMember;

namespace Conectus.Members.IntergrationTests.Application.UseCases.Member.DeleteMember
{
    [Collection(nameof(DeleteMemberTestFixture))]
    public class DeleteMemberTest
    {
        private readonly DeleteMemberTestFixture _fixture;
        public DeleteMemberTest(DeleteMemberTestFixture fixture) => _fixture = fixture;

        [Fact(DisplayName = nameof(DeleteMember))]
        [Trait("Integration/Application", "DeleteMember - Use Cases")]
        public async Task DeleteMember()
        {
            var membersExampleList = _fixture.GetValidMembersList(10);
            var targetGenre = membersExampleList[5];
            var dbArrangeContext = _fixture.CreateDbContext();
            await dbArrangeContext.Members.AddRangeAsync(membersExampleList);
            await dbArrangeContext.SaveChangesAsync();
            var actDbContext = _fixture.CreateDbContext(true);
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var eventPublisher = new DomainEventPublisher(serviceProvider);
            var unitOfWork = new UnitOfWork(
                actDbContext,
                eventPublisher,
                serviceProvider.GetRequiredService<ILogger<UnitOfWork>>());
            var useCase = new UseCase.DeleteMember(
                new MemberRepository(actDbContext),
                unitOfWork
            );
            var input = new UseCase.DeleteMemberInput(targetGenre.Id);

            await useCase.Handle(input, CancellationToken.None);

            var assertDbContext = _fixture.CreateDbContext(true);
            var memberFromDb = await assertDbContext.Members.FindAsync(targetGenre.Id);
            memberFromDb.Should().BeNull();
        }

        [Fact(DisplayName = nameof(ThrowWhenMemberNotFound))]
        [Trait("Application", "DeleteMember - Use Cases")]
        public async Task ThrowWhenMemberNotFound()
        {
            var genresExampleList = _fixture.GetValidMembersList(10);
            var actContext = _fixture.CreateDbContext();
            await actContext.Members.AddRangeAsync(genresExampleList);
            await actContext.SaveChangesAsync();
            var actDbContext = _fixture.CreateDbContext(true); 
            var respository = new MemberRepository(actContext);
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var eventPublisher = new DomainEventPublisher(serviceProvider);
            var unitOfWork = new UnitOfWork(
                actContext,
                eventPublisher,
                serviceProvider.GetRequiredService<ILogger<UnitOfWork>>());
            
            var exampleGuid = Guid.NewGuid();
            var input = new UseCase.DeleteMemberInput(exampleGuid);
            
            var useCase = new UseCase.DeleteMember(
                respository,
                unitOfWork);

            var task = async ()
                => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Member '{exampleGuid}' not found.");
        }
    }
}
