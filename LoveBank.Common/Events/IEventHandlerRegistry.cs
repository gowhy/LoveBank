using System;
using System.Collections.Generic;
using System.Reflection;

namespace LoveBank.Common.Events
{
    public interface IEventHandlerRegistry
    {
        IEnumerable<MethodInfo> FindHandlerMethods(Type eventType);

        bool RegisterHandler(Type handlerType);

        void RegisterHandlers(IEnumerable<Type> handlerTypes);

        void RegisterHandlers(Assembly assembly);

        bool RemoveHandlers(Type eventType);

        void Clear();
    }
}
