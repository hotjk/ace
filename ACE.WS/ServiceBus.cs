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
                throw new Exception("Mapping not found.");
            }

            var client = new RestClient(mapping.BaseUrl);
            var request = new RestRequest(mapping.Resource, Method.GET);
            request.AddObject(req);
            var restResponse = await client.ExecuteTaskAsync(request);
            return JsonConvert.DeserializeObject<TRep>(restResponse.Content);
            //HttpClient httpClient = new HttpClient();
            //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //var values = new List<KeyValuePair<string, string>>();

            //PropertyInfo[] propertyInfos = req.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            //foreach(var pi in propertyInfos)
            //{
            //    request.AddParameter(pi.Name, pi.GetValue(req).ToString()));
            //}
            //var response = await httpClient.PostAsync(mapping.Uri, new FormUrlEncodedContent(values));
            //var jsonString = await response.Content.ReadAsStringAsync();
            //return JsonConvert.DeserializeObject<TRep>(jsonString);
        }
    }
}
