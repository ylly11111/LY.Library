using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LY.MEF.AOP
{
    public class LogInterceptor : Attribute, IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine("begin");
            invocation.Proceed();
            Console.WriteLine("end");
        }
    }
}
