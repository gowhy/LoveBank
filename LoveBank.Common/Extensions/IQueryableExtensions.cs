using System.Collections.Generic;
using System.Linq;

namespace LoveBank.Common
{
    public static class IQueryableExtensions {
        public static JsonPageList<T> ToJsonPageList<T>(this IQueryable<T> source, int total)
                where T : class {
            return new JsonPageList<T>(total, source.ToList());
        }

        public static JsonPageList<T> ToJsonPageList<T>(this List<T> source, int total)
                where T : class {
            return new JsonPageList<T>(total, source);
        }
    }
}
