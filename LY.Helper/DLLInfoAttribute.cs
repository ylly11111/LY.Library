using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LY.Helper
{
    public class DLLInfoAttribute : Attribute
    {
        public string DllName { get; set; }

        public string FullClassName { get; set; }

    }

    [DLLInfo(DllName = "BLL.dll", FullClassName = "BLL.Test")]
     interface ITest
    {
        int Add();
    }

     class Test : ITest
    {

        public int Add()
        {
            return 0;
        }
    }
}
