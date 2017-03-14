using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LY.Messager
{
    public class DefaultPara
    {
        public object SendPara { get; set; }

        public object ReceivePara { get; set; }

        public Action CallAction { get; set; }

    }

    public class DefaultPara<T>
    {

        public Action<T> CallAction { get; set; }
    }
}
