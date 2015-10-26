using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace LoveBank.Common.Unity
{
    public class PerRequestLifetimeMananger:LifetimeManager
    {

        public override object GetValue()
        {
            IDictionary<PerRequestLifetimeMananger, object> lifetimeManagers = PerRequestLifetimeModule.GetPerRequestLifetimeManagers();
            object value;

            lifetimeManagers.TryGetValue(this, out value);

            return value;
        }

        public override void RemoveValue()
        {
            IDictionary<PerRequestLifetimeMananger, object> lifetimeManagers = PerRequestLifetimeModule.GetPerRequestLifetimeManagers();

            object value;

            if (!lifetimeManagers.TryGetValue(this, out value))
            {
                return;
            }

            DisposeValue(value);
            lifetimeManagers.Remove(this);
        }

        public override void SetValue(object newValue)
        {
            if (newValue == null)
            {
                RemoveValue();
                return;
            }

            IDictionary<PerRequestLifetimeMananger, object> lifetimeManagers = PerRequestLifetimeModule.GetPerRequestLifetimeManagers();

            object value;
            if (lifetimeManagers.TryGetValue(this, out value))
            {
                if (value != null && ReferenceEquals(value, newValue))
                {
                    return;
                }
                DisposeValue(value);
            }
            lifetimeManagers[this] = newValue;
        }

        private static void DisposeValue(object value)
        {
            if (value == null)
            {
                return;
            }

            IDisposable disposable = value as IDisposable;

            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
    }
}
