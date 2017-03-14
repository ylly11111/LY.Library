using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LY.Autofac.Test
{
   public class ClassB
    {
       public ClassB()
       {
       }

        private int _Age=29;

        public int Age
        {
            get { return _Age; }
            set { _Age = value; }
        }

    }
}
