using System;
using System.Transactions;

namespace ACE
{
    public class UnitOfWork : IDisposable
    {
        private static TransactionOptions defaultTransactionOptions = new TransactionOptions() { IsolationLevel = IsolationLevel.RepeatableRead };
        private TransactionScope scope;
        private IEventBus _eventBus;

        public UnitOfWork(IEventBus eventBus = null)
        {
            scope = new TransactionScope(TransactionScopeOption.RequiresNew, defaultTransactionOptions);
            _eventBus = eventBus;
        }

        public UnitOfWork(TransactionScopeOption transactionScopeOption, IEventBus eventBus = null)
        {
            scope = new TransactionScope(transactionScopeOption, defaultTransactionOptions);
            _eventBus = eventBus;
        }

        public void Dispose()
        {
            if (_eventBus != null)
            {
                _eventBus.Purge();
            }
            scope.Dispose();
        }

        public void Complete()
        {
            scope.Complete();
            if(_eventBus != null)
            {
                _eventBus.Flush();
            }
        }
    }
}
