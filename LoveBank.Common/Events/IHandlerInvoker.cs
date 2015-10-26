using System.Reflection;

namespace LoveBank.Common.Events
{
    public interface IHandlerInvoker
    {
        void Invoke(IDomainEvent evnt, MethodInfo handlerMethod, EventDispatchingContext context);
    }
}
