using System.Collections.Generic;

namespace LoveBank.Common
{
    public class StaticPagedList<T> : BasePagedList<T>
    {
        public StaticPagedList(IEnumerable<T> source, int index, int pageSize, int totalItemCount)
            : base(index, pageSize, totalItemCount)
        {
            Subset.AddRange(source);
        }
    }
}
