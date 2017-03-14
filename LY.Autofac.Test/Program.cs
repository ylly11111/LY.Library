using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LY.Autofac;
using System.Reflection;
using Autofac;

namespace LY.Autofac.Test
{
    class Program
    {
       static ContainerBuilder Build;
       static IContainer Container;
        static void Main(string[] args)
        {
            IocHelper.Register<ClassA,IClassA>();
            //IocHelper.Register(new ClassB());

            IocHelper.RegisterAssembly(Assembly.GetExecutingAssembly());

            IClassA ca = IocHelper.Resolve<IClassA>();
            ClassB cb = IocHelper.Resolve<ClassB>();

        }
    }
}
