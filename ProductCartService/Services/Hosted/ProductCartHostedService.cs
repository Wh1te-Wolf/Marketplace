using Events;
using Events.Options;
using Events.Services.Interfaces;
using ProductCartService.EventHandlers;

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
                Topic = Topics.MainTopic,
                Actions = new List<ActionFilterStack>()
                { 
                    new ActionFilterStack() 
                    {
                        EventType = "Customer",
                        EventSubType = "Created",
                        Handlers = new List<HandlerConfiguration>() 
                        {
                            new HandlerConfiguration()
                            { 
                                HandlerPath = typeof(CustomerCreatedEventHandler).FullName
                            }
                        }
                    },
                    new ActionFilterStack()
                    {
                        EventType = "Customer",
                        EventSubType = "Removed",
                        Handlers = new List<HandlerConfiguration>()
                        {
                            new HandlerConfiguration()
                            {
                                HandlerPath = typeof(CustomerRemovedEventHandler).FullName
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
