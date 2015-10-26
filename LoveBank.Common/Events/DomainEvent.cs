using System;
using LoveBank.Common.Data;

namespace LoveBank.Common.Events
{
    public abstract class DomainEvent : IDomainEvent
    {

        public DateTime Timestamp { get; private set; }

        protected DomainEvent()
            : this(DateTime.Now)
        {
        }

        protected DomainEvent(DateTime timestamp)
        {
            Timestamp = timestamp;
        }

        public static void Apply<TEvent>(TEvent evnt)
            where TEvent : IDomainEvent
        {
            Check.Argument.IsNotNull(evnt, "evnt");

            var unitOfWork = (UnitOfWorkForEvent)IoC.Resolve<IUnitOfWork>();

            if (unitOfWork == null)
                throw new InvalidOperationException("Domain event can only be applied within a unit of work scope.");

            unitOfWork.ApplyEvent(evnt);
        }

    }
}
