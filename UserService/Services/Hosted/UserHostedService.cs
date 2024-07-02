
using Events.Services.Interfaces;

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
            await _handlerRepository.Initialize();
            await _eventManager.Initialize();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
