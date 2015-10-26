using System;
using System.Reflection;
using System.Threading.Tasks;

namespace LoveBank.Common.Events
{
    public class DefaultHandlerInvoker : IHandlerInvoker
    {
        public void Invoke(IDomainEvent evnt, MethodInfo handlerMethod, EventDispatchingContext context)
        {
            if (TypeUtil.IsAttributeDefinedInMethodOrDeclaringClass(handlerMethod, typeof(HandleAsyncAttribute)))
            {
                Task.Factory.StartNew(() => InvokeHandler(evnt, handlerMethod));
            }
            else
            {
                InvokeHandler(evnt, handlerMethod);
            }
        }

        private void InvokeHandler(IDomainEvent evnt, MethodInfo method)
        {
            var handlerType = method.DeclaringType;
            var handler = CreateHandlerInstance(handlerType);

            try
            {
                method.Invoke(handler, new object[] { evnt });
            }
            catch (Exception ex)
            {
                throw new EventHandlerException("Event handler throws an exception, please check inner exception for detail. Handler type: " + handlerType + ".", ex);
            }
        }

        private object CreateHandlerInstance(Type handlerType)
        {
            try
            {
                var handler = Activator.CreateInstance(handlerType);
                return handler;
            }
            catch (Exception ex)
            {
                throw new EventHandlerException("Failed creating event handler instance. Handler type: " + handlerType + ".", ex);
            }
        }
    }
}
