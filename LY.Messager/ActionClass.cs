using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LY.Messager
{
    public class ActionClass
    {
    }

    public class ActionClass<T> : ActionClass
    {
        public Action<T> action { get; set; }
    }
}
