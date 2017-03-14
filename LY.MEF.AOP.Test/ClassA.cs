using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace LY.MEF.AOP.Test
{
    [LogInterceptor]
    [TestInterceptor]
    [Export(typeof(ClassA))]
    public class ClassA
    {
      public virtual void GetA()
      {
          Console.WriteLine("GetA");
      }
    }
}
