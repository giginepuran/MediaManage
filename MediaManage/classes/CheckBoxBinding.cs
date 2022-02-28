using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaManage.classes
{
    public class CheckBoxBinding
    {
        public string TagName { get; private set; }
        public bool IsChecked { get; set; }

        public CheckBoxBinding(string tagName, bool isChecked)
        {
            TagName = tagName;
            IsChecked = isChecked;
        }
    }
}
