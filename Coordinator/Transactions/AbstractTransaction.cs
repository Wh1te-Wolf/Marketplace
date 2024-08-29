using Events.Entities;
using Events;
using TwoPhaseCommitUtils.Events;
using Events.Services.Interfaces;
using Coordinator.Entities.TransactionData;
using Coordinator.Entities;
using TwoPhaseCommitEntities.Entities.Intarfaces;
using TwoPhaseCommitUtils.Entities.Enums;

namespace Coordinator.Transactions
{
    public abstract class AbstractTransaction
    {
        protected IEventManager _eventManager;

        protected List<Task> _sendingPrepareEventTasks = new();

        protected List<Task> _sendingCommitEventTasks = new();

        protected Dictionary<string, List<string>> _serviceTypeMappings = new();

        public Guid UUID { get; set; }

        protected AbstractTransaction(IEventManager eventManager)
        {
            _eventManager = eventManager;
        }

        public async Task SendPrepareEventsAsync()
            => await Task.WhenAll(_sendingPrepareEventTasks);

        protected string GetTarget(string serviceName, string type) => $"{serviceName}.{type}";

        protected void AddPrepareTask(string type, Guid uuid, string serviceName)
        {
            PrepareEventData prepareEventData = new PrepareEventData()
            {
                ObjectType = type,
                ObjectUUID = uuid,
                TransactionUUID = UUID
            };

            MarketplaceEventBase prepareEvent = new MarketplaceEventBase(serviceName, $"{type}.Prepare", prepareEventData);

            Task prepareTask = _eventManager.ProduceAsync(Topics.TwoPhaseCommitTopic, prepareEvent);
            _sendingPrepareEventTasks.Add(prepareTask);
        }

        protected void AddCommitTask(string type, string serviceName, IEntityCommitData entityCommitData, CUDAction action)
        {
            CommitEventData commitEventData = new CommitEventData()
            {
                TransactionUUID = UUID,
                Action = action,
                EntityCommitData = entityCommitData,
                EntityType = type
            };

            MarketplaceEventBase commitEvent = new MarketplaceEventBase(serviceName, $"{type}.Commit", commitEventData);

            Task commitTask = _eventManager.ProduceAsync(Topics.TwoPhaseCommitTopic, commitEvent);
            _sendingCommitEventTasks.Add(commitTask);
        }

        protected async Task ReleaseLockForEntityAsync(string type, Guid uuid)
        {
            ReleaseLockData customerReleaseLockData = new ReleaseLockData()
            {
                UUID = uuid,
                Type = type
            };

            MarketplaceEventBase releaseCustomerLockEvent = new MarketplaceEventBase("ReleaseLock", type, customerReleaseLockData);
            await _eventManager.ProduceAsync(Topics.TwoPhaseCommitTopic, releaseCustomerLockEvent);
        }

        protected TransactionLocalRequestInfo GetTransactionLocalRequestInfo(string serviceName, string typeName)
            => new TransactionLocalRequestInfo()
            {
                TransactionUUID = UUID,
                TaskCompletionSource = new TaskCompletionSource<bool>(),
                Result = false,
                Target = GetTarget(serviceName, typeName)
            };
    }
}
