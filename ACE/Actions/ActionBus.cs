using ACE.Loggers;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Autofac;

namespace ACE
{
    public class ActionBus : IActionBus
    {
        private EasyNetQ.IBus _bus;
        private IBusLogger _busLogger;

        public ActionBus(IBusLogger busLogger, 
            EasyNetQ.IBus bus = null)
        {
            _bus = bus;
            _busLogger = busLogger;
        }

        public ActionResponse Send<B, T>(T action)
            where B : class, IAction
            where T : B
        {
            _busLogger.Sent(action);
            ActionResponse response = _bus.Request<B, ActionResponse>(action);
            _busLogger.Received(response);
            return response;
        }

        public async Task<ActionResponse> SendAsync<B, T>(T action)
            where B : class, IAction
            where T : B
        {
            _busLogger.Sent(action);
            ActionResponse response = await _bus.RequestAsync<B, ActionResponse>(action);
            _busLogger.Received(response);
            return response;
        }

        public async Task<ActionResponse> SendAsyncWithRetry<B, T>(T action, int retryCount = 2)
            where B : class, IAction
            where T : B
        {
            ActionResponse response = null;
            int currentRetry = 0;
            while (true)
            {
                try
                {
                    response = await SendAsync<B, T>(action);
                    break;
                }
                catch (Exception ex)
                {
                    if (ex is ACE.Exceptions.BusinessException)
                    {
                        throw;
                    }
                    currentRetry++;
                    if (currentRetry > retryCount)
                    {
                        throw;
                    }
                }
                await Task.Delay(100);
            }
            return response;
        }
    }
}
