using Events;
using Events.Attributes;
using Events.Entities;
using Events.Handlers;
using Events.Services.Interfaces;
using SagaEventData.EventData;
using UserService.Entities;
using UserService.Services;
using UserService.Services.Interfaces;

namespace UserService.EventHandlers.CustomerRemoveSaga
{
    [MarketplaceEventHandler]
    public class CreateUserHandler : IEventHandler
    {
        private readonly ICustomerService _customerService;
        private readonly IEventManager _eventManager;

        public CreateUserHandler(ICustomerService customerService, IEventManager eventManager)
        {
            _customerService = customerService;
            _eventManager = eventManager;
        }

        public async Task HandleAsync(IMarketplaceEvent marketplaceEvent)
        {
            try
            {
                CustomerSagaEventData sagaEventData = marketplaceEvent.EventData as CustomerSagaEventData;
                Customer customer = new Customer()
                { 
                    Name = sagaEventData.Name,
                    PhoneNumber = sagaEventData.PhoneNumber,
                    Email = sagaEventData.Email,
                    UUID = sagaEventData.UUID,
                    Surname = sagaEventData.Surname,
                };

                await _customerService.AddAsync(customer);

                MarketplaceEventBase marketplaceEventBase = new MarketplaceEventBase("CustomerRemoveSaga", "CustomerRemove.Rollback", sagaEventData);
                await _eventManager.ProduceAsync(Topics.TransactionTopic, marketplaceEventBase);
            }
            catch
            { 
                //TODO сделать логи.
            }
        }
    }
}
