using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ACE.WS
{
    public class ServiceMapping
    {
        public ServiceMapping(string baseUrl, string resource)
        {
            this.BaseUrl = baseUrl;
            this.Resource = resource;
        }

        public string BaseUrl { get; private set; }
        public string Resource { get; private set; }
    }
}
