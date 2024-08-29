using Events.Attributes;
using Events.Entities;
using Events.Handlers;
using Events.Services.Interfaces;
using Events;
using SagaEventData.EventData;
using UserService.Entities;
using UserService.Services.Interfaces;
using System.Data;

namespace UserService.EventHandlers.CustomerCreateSaga
{
    [MarketplaceEventHandler]
    public class CreateUserSagaHandler : IEventHandler
    {
        private readonly ICustomerService _customerService;
        private readonly IEventManager _eventManager;

        public CreateUserSagaHandler(ICustomerService customerService, IEventManager eventManager)
        {
            _customerService = customerService;
            _eventManager = eventManager;
        }

        public async Task HandleAsync(IMarketplaceEvent marketplaceEvent)
        {
            CustomerSagaEventData sagaEventData = marketplaceEvent.EventData as CustomerSagaEventData;

            try
            {
                Customer customer = new Customer()
                {
                    Name = sagaEventData.Name,
                    PhoneNumber = sagaEventData.PhoneNumber,
                    Email = sagaEventData.Email,
                    UUID = sagaEventData.UUID,
                    Surname = sagaEventData.Surname,
                };

                await _customerService.AddAsync(customer);

                MarketplaceEventBase marketplaceEventBase = new MarketplaceEventBase("CustomerCreateSaga", "CustomerCreate._commited", sagaEventData);
                await _eventManager.ProduceAsync(Topics.TransactionTopic, marketplaceEventBase);
            }
            catch
            {
                MarketplaceEventBase marketplaceEventBase = new MarketplaceEventBase("CustomerCreateSaga", "CustomerCreate._rollback", sagaEventData);
                await _eventManager.ProduceAsync(Topics.TransactionTopic, marketplaceEventBase);
            }
        }
    }
}