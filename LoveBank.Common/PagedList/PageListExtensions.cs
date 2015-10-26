using System.Collections.Generic;

namespace LoveBank.Common
{
    public static class PageListExtensions
    {
        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> source,int index, int pageSize) where T:class
        {
            var pageList = new PagedList<T>(source, index, pageSize);
            return pageList;
        } 
    }
}
