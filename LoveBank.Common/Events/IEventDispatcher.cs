namespace LoveBank.Common.Events
{
    public interface IEventDispatcher
    {
        void Dispatch(IDomainEvent evnt, EventDispatchingContext context);
    }
}
