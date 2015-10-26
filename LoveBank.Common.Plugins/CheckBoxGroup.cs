using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QDT.Common.Plugins
{
    public class CheckBoxGroup : Dictionary<string, CheckBox>
    {

        public CheckBoxGroup Add(string key, string value)
        {
            if (this.ContainsKey(key))
            {
                this[key].Value = value;
            }
            else
            {
                this.Add(key, new CheckBox(key, value));
            }
            return this;
        }

        public void SetChecked(string key, bool isSelect)
        {

            if (!this.ContainsKey(key)) return;

            this[key].Checked = isSelect;
        }
    }
}
