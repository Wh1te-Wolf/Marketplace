using Events.Options;
using Events.Services.Interfaces;
using Events;
using Coordinator.Handlers;

namespace Coordinator.Services.Hosted
{
    public class CoordinatorHostedService : IHostedService
    {
        private readonly IEventManager _eventManager;
        private readonly IHandlerRepository _handlerRepository;

        public CoordinatorHostedService(IEventManager eventManager, IHandlerRepository handlerRepository)
        {
            _eventManager = eventManager;
            _handlerRepository = handlerRepository;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            TopicFilterStack topicFilterStack = new TopicFilterStack()
            {
                Topic = Topics.TwoPhaseCommitTopic,
                Actions = new List<ActionFilterStack>()
                {
                    new ActionFilterStack()
                    {
                        EventType = "TwoPhaseCommit",
                        EventSubType = "Prepare.Success",
                        Handlers = new List<HandlerConfiguration>()
                        {
                            new HandlerConfiguration()
                            {
                                HandlerPath = typeof(PrepareSuccessResponseHandler).FullName
                            }
                        }
                    },
                    new ActionFilterStack()
                    {
                        EventType = "TwoPhaseCommit",
                        EventSubType = "Prepare.Failure",
                        Handlers = new List<HandlerConfiguration>()
                        {
                            new HandlerConfiguration()
                            {
                                HandlerPath = typeof(PrepareFailureResponseHandler).FullName
                            }
                        }
                    },
                    new ActionFilterStack()
                    {
                        EventType = "TwoPhaseCommit",
                        EventSubType = "Commit.Success",
                        Handlers = new List<HandlerConfiguration>()
                        {
                            new HandlerConfiguration()
                            {
                                HandlerPath = typeof(CommitSuccesResponseHandler).FullName
                            }
                        }
                    },
                    new ActionFilterStack()
                    {
                        EventType = "TwoPhaseCommit",
                        EventSubType = "Commit.Failure",
                        Handlers = new List<HandlerConfiguration>()
                        {
                            new HandlerConfiguration()
                            {
                                HandlerPath = typeof(CommitFailureResponseHandler).FullName
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
