using Grit.CQRS.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.CQRS.Actions
{
    public class ActionWorker
    {
        public ActionResponse Execute(Grit.CQRS.Action action)
        {
            ActionResponse response = new ActionResponse { Result = ActionResponse.ActionResponseResult.OK };
            try
            {
                ServiceLocator.ActionBus.Invoke((dynamic)action);
            }
            catch (BusinessException ex)
            {
                response.Result = ActionResponse.ActionResponseResult.NG;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
