using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoveBank.Common.Events
{
    public interface IDomainEvent
    {
        DateTime Timestamp { get; }
    }
}
