using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LY.Autofac
{
    public class IocHelper
    {
        private static IContainer _Container;

        public static IContainer Container
        {
            get 
            {
                if (_Container == null)
                {
                    _Container = _Build.Build();
                }
                return _Container; 
            }
        }


        private static ContainerBuilder _Build ;

        public static ContainerBuilder CurrentBuild
        {
            get 
            { 
                return _Build;
            }
        }

        static IocHelper()
        {
            _Build = new ContainerBuilder();
        }

        public static void Register<T>()
        {
            _Build.RegisterType<T>();
        }

        public static void Register<T,T1>()
        {
            _Build.RegisterType<T>().As<T1>();
        }

        public static void Register(object obj)
        {
            _Build.RegisterInstance(obj);
        }

        public static void RegisterAssembly(Assembly assembly)
        {
            _Build.RegisterAssemblyTypes(assembly);
        }

        public static void RegisterAssemblys(Assembly[] Assemblys)
        {
            _Build.RegisterAssemblyTypes(Assemblys);
        }

        public static T Resolve<T>(string name)
        {
            return Container.ResolveNamed<T>(name);
        }

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }
    }
}
