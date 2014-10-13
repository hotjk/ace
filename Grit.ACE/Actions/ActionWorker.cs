using Grit.ACE.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.ACE.Actions
{
    public class ActionWorker
    {
        public ActionResponse Execute(Grit.ACE.Action action)
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
