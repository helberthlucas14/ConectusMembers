using Conectus.Members.Application;
using Conectus.Members.Application.Exceptions;
using Conectus.Members.Infra.Data.EF;
using Conectus.Members.Infra.Data.EF.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
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
            var actContext = _fixture.CreateDbContext();
            var exampleMember = _fixture.GetValidMemberExample();
            var exampleList = _fixture.GetValidMembersList(10);

            await actContext.AddRangeAsync(exampleList);
            var trackingInfo = await actContext.AddAsync(exampleMember);

            await actContext.SaveChangesAsync();
            trackingInfo.State = EntityState.Detached;

            var respository = new MemberRepository(actContext);
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var eventPublisher = new DomainEventPublisher(serviceProvider);
            var unitOfWork = new UnitOfWork(
                actContext,
                eventPublisher,
                serviceProvider.GetRequiredService<ILogger<UnitOfWork>>());

            var input = new UseCase.DeleteMemberInput(exampleMember.Id);

            var useCase = new UseCase.DeleteMember(
                respository,
                unitOfWork);

            await useCase.Handle(input, CancellationToken.None);

            var assertDbContext = _fixture.CreateDbContext(true);
            var dbMember = await assertDbContext
                                  .Members.FindAsync(exampleMember.Id);

            dbMember.Should().BeNull();
            var dbCategories = await assertDbContext.Members.ToListAsync();
            dbCategories.Should().HaveCount(exampleList.Count);
        }

        [Fact(DisplayName = nameof(ThrowWhenMemberNotFound))]
        [Trait("Application", "DeleteMember - Use Cases")]
        public async Task ThrowWhenMemberNotFound()
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
