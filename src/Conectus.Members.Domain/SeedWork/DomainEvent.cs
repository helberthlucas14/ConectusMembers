namespace Conectus.Members.Domain.SeedWork;
public abstract class DomainEvent
{
    public DateTime OccuredOn { get; set; }
    protected DomainEvent()
    {
        OccuredOn = DateTime.Now;
    }
}
