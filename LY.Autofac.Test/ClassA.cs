using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LY.Autofac.Test
{
   public class ClassA:IClassA
    {
       public ClassA()
       {
       }

        public string Name { get; set; }

        public string GetName()
        {
            Name = "aaa";
            return Name;
        }
    }
}
