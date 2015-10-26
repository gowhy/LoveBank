using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QDT.Common.Plugins
{
    [Serializable]
    public class CheckBox
    {
        public CheckBox() {
            
        }

        public CheckBox(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }

        public string Value { get; set; }

        public bool Checked { get; set; }
    }
}
