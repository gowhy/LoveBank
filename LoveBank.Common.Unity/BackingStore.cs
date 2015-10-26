using System.Collections;
using System.Web;
using System.Threading;

namespace LoveBank.Common.Unity
{
    internal static class BackingStore
    {
        private static readonly string Key = typeof(BackingStore).FullName;

        public static IDictionary Get()
        {
            IDictionary backingStore = (HttpContext.Current != null) ? HttpContext.Current.Items : null;

            if (HttpContext.Current == null)
            {
                backingStore = Thread.GetData(Thread.GetNamedDataSlot(Key)) as IDictionary;

                if (backingStore == null)
                {
                    backingStore = new Hashtable();
                    Thread.SetData(Thread.GetNamedDataSlot(Key), backingStore);
                }
            }

            return backingStore;
        }
    }
}
