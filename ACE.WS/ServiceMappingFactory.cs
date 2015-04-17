using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.WS
{
    public class ServiceMappingFactory : IServiceMappingFactory
    {
        private readonly object _lockThis = new object();
        private IDictionary<Type, ServiceMapping> _mappings;

        public ServiceMappingFactory(Func<IDictionary<Type, ServiceMapping>> func)
        {
            lock (_lockThis)
            {
                _mappings = func();
            }
        }

        public ServiceMapping GetServiceMapping(Type type)
        {
            ServiceMapping value;
            if(_mappings.TryGetValue(type, out value))
            {
                return value;
            }
            return null;
        }
    }
}
