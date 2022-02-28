﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaManage.classes
{
    public struct Tag
    {
        public Tag(string tagName)
        {
            this.TagName = tagName;
        }

        public string TagName { get; private set; }
    }
}