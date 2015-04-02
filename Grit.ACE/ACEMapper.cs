using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE
{
    public static class ACEMapper
    {
        public static IMappingExpression<TSource, TDestination> CreateMapIgnoreId<TSource, TDestination>()
            where TDestination : DomainMessage
        {
            return ACEMapper.CreateMapIgnoreId<TSource, TDestination>();
        }

        public static IMappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>()
        {
            return ACEMapper.CreateMap<TSource, TDestination>();
        }

        public static TDestination Map<TDestination>(object source)
        {
            return ACEMapper.Map<TDestination>(source);
        }
    }
}
