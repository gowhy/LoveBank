using System;

namespace LoveBank.Common.Events
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class AwaitCommittedAttribute : Attribute
    {
    }
}
