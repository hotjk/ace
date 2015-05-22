using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ACE.WS
{
    public class ServiceBus : IServiceBus
    {
        private IServiceMappingFactory _serviceMappingFactory;

        public ServiceBus(IServiceMappingFactory serviceMappingFactory)
        {
            _serviceMappingFactory = serviceMappingFactory;
        }

        public async Task<TRep> InvokeAsync<TReq, TRep>(TReq req) where TReq : IService
        {
            var mapping = _serviceMappingFactory.GetServiceMapping(req.GetType());
            if (mapping == null)
            {
                throw new ServiceException("ServiceBus mapping not found.");
            }

            RestClient client = null;
            RestRequest request = null;
            IRestResponse restResponse;

            try
            {
                client = new RestClient(mapping.BaseUrl);
                request = new RestRequest(mapping.Resource, Method.GET);
                request.AddObject(req);
                restResponse = await client.ExecuteTaskAsync(request);
            }
            catch(Exception ex)
            {
                throw new ServiceException(string.Format("ServiceBus.Invoke {0}, Return: {1}, Url: {2}",
                    req.GetType(),
                    ex.Message,
                    mapping.Url), ex);
            }
            if(restResponse.ErrorException != null)
            {
                throw new ServiceException(string.Format("ServiceBus.Invoke {0}, Return: {1}, Url: {2}",
                    req.GetType(),
                    restResponse.ErrorMessage,
                    mapping.Url), restResponse.ErrorException);
            }
            if (restResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new ServiceException(string.Format("ServiceBus.Invoke {0}, Return: {1}, Url: {2}",
                    req.GetType(),
                    restResponse.StatusCode,
                    mapping.Url));
            }
            return Newtonsoft.Json.JsonConvert.DeserializeObject<TRep>(restResponse.Content);
        }
    }
}
