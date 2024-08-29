using Events.Options;
using Events;
using Events.Services.Interfaces;
using UserService.EventHandlers.CustomerRemoveSaga;
using UserService.EventHandlers.CustomerCreateSaga;

namespace UserService.Services.Hosted
{
    public class UserHostedService : IHostedService
    {
        private readonly IEventManager _eventManager;
        private readonly IHandlerRepository _handlerRepository;

        public UserHostedService(IEventManager eventManager, IHandlerRepository handlerRepository)
        {
            _eventManager = eventManager;
            _handlerRepository = handlerRepository;
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
                        EventSubType = "StartCustomerRemoveSaga",
                        Handlers = new List<HandlerConfiguration>()
                        {
                            new HandlerConfiguration()
                            {
                                HandlerPath = typeof(RemoveUserHandler).FullName
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
                                HandlerPath = typeof(CreateUserHandler).FullName
                            }
                        }
                    },
                    new ActionFilterStack()
                    {
                        EventType = "CustomerCreateSaga",
                        EventSubType = "ProductCartCreate._rollback",
                        Handlers = new List<HandlerConfiguration>()
                        {
                            new HandlerConfiguration()
                            {
                                HandlerPath = typeof(RemoveUserSagaHandler).FullName
                            }
                        }
                    },
                    new ActionFilterStack()
                    {
                        EventType = "CustomerCreateSaga",
                        EventSubType = "StartCustomerCreateSaga",
                        Handlers = new List<HandlerConfiguration>()
                        {
                            new HandlerConfiguration()
                            {
                                HandlerPath = typeof(CreateUserSagaHandler).FullName
                            }
                        }
                    }
                }
            };

            await _handlerRepository.Initialize();
            _eventManager.AddTopicFilterStack(topicFilterStack);
            await _eventManager.Initialize();
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
