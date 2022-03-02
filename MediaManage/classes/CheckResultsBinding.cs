using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaManage.classes
{
    public class CheckResultBinding
    {
        public string Item { get; private set; }
        public bool IsOk { get; private set; }
        public string Info { get; private set; }

        public CheckResultBinding(string item, bool isOk, string info)
        {
            Item = item;
            IsOk = isOk;
            Info = info;
        }
    }
}
