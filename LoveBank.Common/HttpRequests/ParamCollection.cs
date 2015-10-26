using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LoveBank.Common
{
    public class ParamCollection : Collection<ParamPair>
    {
        public ParamCollection()
        {
        }

        public ParamCollection(IEnumerable<ParamPair> items)
        {
            foreach (ParamPair item in items)
            {
                Add(item.Name, item.Value);
            }
        }

        public void Add(string name, string value)
        {
            base.Add(new ParamPair(name, value));
        }
    }
}