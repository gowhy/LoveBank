using System.Collections.Generic;
using System.Linq;

namespace LoveBank.Common {
    public class JsonPageList<T> where T : class {

        public JsonPageList(int total, List<T> source) {
            rows = source.ToList();
            this.total = total;
        }
        public int total { private set; get; }
        public List<T> rows { private set; get; }
    }
}
