using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoveBank.Common.Events
{
    public class DefaultEventDispatcher : IEventDispatcher
    {
        private readonly IEventHandlerRegistry _handlerRegistry;
        private readonly IHandlerInvoker _handlerInvoker;

        public IEventHandlerRegistry HandlerRegistry
        {
            get
            {
                return _handlerRegistry;
            }
        }

        public IHandlerInvoker HandlerInvoker
        {
            get
            {
                return _handlerInvoker;
            }
        }

        public DefaultEventDispatcher()
            : this(new DefaultEventHandlerRegistry())
        {
        }

        public DefaultEventDispatcher(IEventHandlerRegistry handlerRegistry)
            : this(handlerRegistry, new DefaultHandlerInvoker())
        {

        }

        public DefaultEventDispatcher(IEventHandlerRegistry handlerRegistry, IHandlerInvoker handlerInvoker)
        {
            Check.Argument.IsNotNull(handlerRegistry, "handlerRegistry");
            Check.Argument.IsNotNull(handlerInvoker, "handlerInvoker");

            _handlerRegistry = handlerRegistry;
            _handlerInvoker = handlerInvoker;
        }

        public void Dispatch(IDomainEvent evnt, EventDispatchingContext context)
        {
            Check.Argument.IsNotNull(evnt, "evnt");
            Check.Argument.IsNotNull(context, "context");

            foreach (var method in _handlerRegistry.FindHandlerMethods(evnt.GetType()))
            {
                var awaitCommit = TypeUtil.IsAttributeDefinedInMethodOrDeclaringClass(method, typeof(AwaitCommittedAttribute));

                if (awaitCommit && !context.WasUnitOfWorkCommitted
                    || !awaitCommit && context.WasUnitOfWorkCommitted)
                {
                    continue;
                }

                _handlerInvoker.Invoke(evnt, method, context);
            }
        }
    }
}
