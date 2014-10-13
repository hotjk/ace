using ACE.Demo.Contracts.Events;
using ACE.Demo.Model.Messages;
using Grit.ACE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Demo.Model.Write.Messages
{
    public class MessageHandler : 
        IEventHandler<InvestmentStatusCreated>,
        IEventHandler<AccountAmountChanged>
    {
        private IMessageWriteRepository _repository;
        public MessageHandler(IMessageWriteRepository repository)
        {
            _repository = repository;
        }

        public void Handle(InvestmentStatusCreated handle)
        {
            _repository.Add(new Message
            {
                AccountId = handle.AccountId,
                Content = string.Format("投资成功，投资金额{0:n}元。", handle.Amount)
            });
        }

        public void Handle(AccountAmountChanged handle)
        {
            _repository.Add(new Message
            {
                AccountId = handle.AccountId,
                Content = string.Format("账户变动，变动金额{0:n}元。", handle.Change)
            });
        }
    }
}
