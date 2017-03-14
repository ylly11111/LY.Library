using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LY.MEF.AOP
{
    public abstract class InterceptorBase:Attribute,IInterceptor
    {
        public  void Intercept(IInvocation invocation)
        {
            BeforeOperate();
            invocation.Proceed();
            AfterOperate();
        }

        public abstract void BeforeOperate();

        public abstract void AfterOperate();
    }
}
