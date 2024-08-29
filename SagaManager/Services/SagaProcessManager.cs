using Events;
using Events.Data;
using Events.Data.Interfaces;
using Events.Entities;
using Events.Services.Interfaces;
using SagaManager.Services.Interfaces;
using System.Collections.Concurrent;

namespace SagaManager.Services
{
    public class SagaProcessManager : ISagaProcessManager
    {
        private class SagaRequestInfo 
        {
            public Guid TransactionUUID { get; set; }

            public TaskCompletionSource<bool> TaskCompletionSource { get; set; }

            public bool Result { get; set; } = false;

            public SagaRequestInfo(Guid transactionUUID, TaskCompletionSource<bool> taskCompletionSource, bool result = false )
            {
                TransactionUUID = transactionUUID;
                TaskCompletionSource = taskCompletionSource;
                Result = result;
            }

        }

        private readonly ConcurrentDictionary<Guid, SagaRequestInfo> _pendingTransactions = new ConcurrentDictionary<Guid, SagaRequestInfo>();
        private Dictionary<string, Type> _startingEvents = new Dictionary<string, Type>();
        private readonly IEventManager _eventManager;
        private const int _transactionTimeout = 10000;

        public SagaProcessManager(IEventManager eventManager)
        {
            _eventManager = eventManager;
        }

        public void AddSagaInformation(Guid transactionUUID, bool result)
        {
            if (!_pendingTransactions.TryGetValue(transactionUUID, out SagaRequestInfo sagaInfo))
                return;
            sagaInfo.Result = result;
            sagaInfo.TaskCompletionSource.SetResult(result);
        }

        public async Task<bool> ProcessAsync(string sagaName, ITransactionEventData sagaData)
        {
            Guid transactionUUID = Guid.NewGuid();

            Type eventType = _startingEvents[sagaName];
            IMarketplaceEvent? createdEvent = Activator.CreateInstance(eventType) as IMarketplaceEvent;
            createdEvent.EventData = sagaData;
            sagaData.TransactionUUID = transactionUUID;

            TaskCompletionSource<bool> taskCompletionSource = new();
            SagaRequestInfo sagaRequestInfo = new SagaRequestInfo(transactionUUID, taskCompletionSource);
            _pendingTransactions.TryAdd(transactionUUID, sagaRequestInfo);

            await _eventManager.ProduceAsync(Topics.TransactionTopic, createdEvent);
            var responseTask = await Task.WhenAny(taskCompletionSource.Task, Task.Delay(_transactionTimeout));
            _pendingTransactions.TryRemove(transactionUUID, out _);
            if(responseTask != taskCompletionSource.Task)
                return false;
            return sagaRequestInfo.Result;
        }

        public void AddStartingEvent(string sagaName, Type type)
        {
            _startingEvents.Add(sagaName, type);
        }
    }
}
