using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.WS
{
    public interface IServiceBus
    {
        Task<TRep> InvokeAsync<TReq, TRep>(TReq req) where TReq : IService;
    }
}
