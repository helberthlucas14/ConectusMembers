namespace Conectus.Members.Domain.SeedWork
{
    public interface IDomainEventHandler<TDomainEvent> where TDomainEvent : DomainEvent
    {
        Task HandleAsync(TDomainEvent domainEvent, CancellationToken cancellationToken);
    }


}
