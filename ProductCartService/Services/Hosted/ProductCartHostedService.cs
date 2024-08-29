using Events;
using Events.Options;
using Events.Services.Interfaces;
using ProductCartService.EventHandlers;
using ProductCartService.EventHandlers.CustomerRemoveSaga;

namespace ProductCartService.Services.Hosted
{
    public class ProductCartHostedService : IHostedService
    {
        private readonly IEventManager _eventManager;
        private readonly IHandlerRepository _handlerRepository;

        public ProductCartHostedService(IEventManager eventManager, IHandlerRepository handlerRepository)
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
                        EventSubType = "CustomerRemove.Commited",
                        Handlers = new List<HandlerConfiguration>() 
                        {
                            new HandlerConfiguration()
                            { 
                                HandlerPath = typeof(RemoveProductCardHandler).FullName
                            }
                        }
                    },
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
