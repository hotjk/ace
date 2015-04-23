using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Utility.Mapper
{
    public static class MappingExpressionExtensions
    {
        public static IMappingExpression<TSource, TDest> IgnoreIdMap<TSource, TDest>(this IMappingExpression<TSource, TDest> expression)
            where TSource:DomainMessage
        {
            expression.ForAllMembers(opt => opt.Ignore());
            return expression;
        }
    }
}
