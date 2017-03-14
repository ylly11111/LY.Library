using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LY.MEF.AOP.Test
{
   public class TestInterceptor:InterceptorBase
    {

        public override void BeforeOperate()
        {
            Console.WriteLine("1") ;
        }

        public override void AfterOperate()
        {
            Console.WriteLine("2");
        }
    }
}
