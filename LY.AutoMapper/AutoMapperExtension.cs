using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using System.Collections;

namespace LY.AutoMapper
{
    public static class AutoMapperExtension
    {
        public static TDestination MapTo<TDestination>(this object o)
        {
            if (o == null)
                throw new ArgumentNullException();

            Mapper.CreateMap(o.GetType(),typeof(TDestination));

            return Mapper.Map<TDestination>(o); ;
        }

        public static List<TDestination> MapTo<TDestination>(this IEnumerable o)
        {
            if (o == null)
                throw new ArgumentNullException();
            

            foreach(var item in o)
            {
                Mapper.CreateMap(item.GetType(),typeof(TDestination));

                break;
            }
            return Mapper.Map<List<TDestination>>(o);
        }
    }
}
