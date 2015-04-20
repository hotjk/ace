using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Demo.Model
{
    public static class ACEMapper
    {
        public static IMappingExpression<TSource, TDestination> CreateMapIgnoreId<TSource, TDestination>()
            where TDestination : DomainMessage
        {
            return AutoMapper.Mapper.CreateMap<TSource, TDestination>().ForMember(dest => dest._id, opt => opt.Ignore());
        }

        public static IMappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>()
        {
            return AutoMapper.Mapper.CreateMap<TSource, TDestination>();
        }

        public static TDestination Map<TDestination>(object source)
        {
            return AutoMapper.Mapper.Map<TDestination>(source);
        }
    }
}
