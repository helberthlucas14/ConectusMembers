using Bogus.DataSets;
using Conectus.Members.Application;
using Conectus.Members.Domain.SeedWork;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using UnitOfWorkInfra = Conectus.Members.Infra.Data.EF;

namespace Conectus.Members.IntergrationTests.Infra.Data.EF.UnitOfWork
{
    [Collection(nameof(UnitOfWorkTestFixture))]
    public class UnitOfWorkTest
    {
        private readonly UnitOfWorkTestFixture _fixture;

        public UnitOfWorkTest(UnitOfWorkTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = nameof(Commit))]
        [Trait("Integration/Infra.Data", "UnitOfWork - Persistence")]
        public async Task Commit()
        {
            var dbName = "test";
            var dbContext = _fixture.CreateDbContext(dbName: dbName);
            var exampleMembersList = _fixture.GetExampleMembersList();
            var memberWithEvent = exampleMembersList.First();
            var @event = new DomainEventFake();
            memberWithEvent.RaiseEvent(@event);
            var eventHandlerMock = new Mock<IDomainEventHandler<DomainEventFake>>();
            await dbContext.AddRangeAsync(exampleMembersList);
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();
            serviceCollection.AddSingleton(eventHandlerMock.Object);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var eventPublisher = new DomainEventPublisher(serviceProvider);
            var unitOfWork = new UnitOfWorkInfra.UnitOfWork(dbContext,
                eventPublisher,
                serviceProvider.GetRequiredService<ILogger<UnitOfWorkInfra.UnitOfWork>>());

            await unitOfWork.Commit(CancellationToken.None);

            var assertDbContext = _fixture.CreateDbContext(true, dbName: dbName);

            var savedMembers = assertDbContext.Members
                .AsNoTracking().ToList();
            savedMembers.Should()
                .HaveCount(exampleMembersList.Count);

            eventHandlerMock.Verify(x =>
                x.HandleAsync(@event, It.IsAny<CancellationToken>()),
                Times.Once);
            memberWithEvent.Events.Should().BeEmpty();
        }


        [Fact(DisplayName = nameof(Rollback))]
        [Trait("Integration/Infra.Data", "UnitOfWork - Persistence")]
        public async Task Rollback()
        {
            var dbContext = _fixture.CreateDbContext();
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var eventPublisher = new DomainEventPublisher(serviceProvider);
            var unitOfWork = new UnitOfWorkInfra.UnitOfWork(dbContext,
                eventPublisher,
                serviceProvider
                .GetRequiredService<ILogger<UnitOfWorkInfra.UnitOfWork>>());

            var task = async ()
                => await unitOfWork.Rollback(CancellationToken.None);

            await task.Should().NotThrowAsync();
        }
    }
}
