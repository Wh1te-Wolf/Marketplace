using Events.Options;
using Events;
using Events.Services.Interfaces;
using SagaManager.Handlers.CustomerRemoveSaga;
using SagaManager.Services.Interfaces;
using SagaEventData.Events;
using SagaManager.Handlers.CustomerCreateSaga;

namespace SagaManager.Services.Hosted
{
    public class SagaManagerHostedService : IHostedService
    {
        private readonly IEventManager _eventManager;
        private readonly IHandlerRepository _handlerRepository;
        private readonly ISagaProcessManager _sagaProcessManager;

        public SagaManagerHostedService(IEventManager eventManager, IHandlerRepository handlerRepository, ISagaProcessManager sagaProcessManager)
        {
            _eventManager = eventManager;
            _handlerRepository = handlerRepository;
            _sagaProcessManager = sagaProcessManager;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            TopicFilterStack topicFilterStack = new TopicFilterStack()
            {
                Topic = Topics.TransactionTopic,
                Actions = new List<ActionFilterStack>()
                {
                    new ActionFilterStack()
                    {
                        EventType = "CustomerRemoveSaga",
                        EventSubType = "ProductCartRemove.Commited",
                        Handlers = new List<HandlerConfiguration>()
                        {
                            new HandlerConfiguration()
                            {
                                HandlerPath = typeof(ProductCartRemoveCommitedHandler).FullName
                            }
                        }
                    },
                    new ActionFilterStack()
                    {
                        EventType = "CustomerRemoveSaga",
                        EventSubType = "CustomerRemove.Rollback",
                        Handlers = new List<HandlerConfiguration>()
                        {
                            new HandlerConfiguration()
                            {
                                HandlerPath = typeof(CustomerRemoveRollbackHandler).FullName
                            }
                        }
                    },
                    new ActionFilterStack()
                    {
                        EventType = "CustomerCreateSaga",
                        EventSubType = "ProductCartCreate._commited",
                        Handlers = new List<HandlerConfiguration>()
                        {
                            new HandlerConfiguration()
                            {
                                HandlerPath = typeof(ProductCartCreatedCommitedHandler).FullName
                            }
                        }
                    },
                    new ActionFilterStack()
                    {
                        EventType = "CustomerCreateSaga",
                        EventSubType = "CustomerCreate._rollback",
                        Handlers = new List<HandlerConfiguration>()
                        {
                            new HandlerConfiguration()
                            {
                                HandlerPath = typeof(CustomerRemoveRollbackHandler).FullName
                            }
                        }
                    }

                }
            };

            await _handlerRepository.Initialize();
            _eventManager.AddTopicFilterStack(topicFilterStack);
            await _eventManager.Initialize();
            _sagaProcessManager.AddStartingEvent("CustomerRemoveSaga", typeof(StartRemoveCustomerSagaEvent));
            _sagaProcessManager.AddStartingEvent("CustomerCreateSaga", typeof(StartCreateCustomerSagaEvent));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
