namespace LoveBank.Common.Events
{
    public interface IHandle<in TEvent>
        where TEvent : IDomainEvent
    {
        void Handle(TEvent evnt);
    }
}
