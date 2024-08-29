using Events;
using Events.Attributes;
using Events.Entities;
using Events.Handlers;
using Events.Services.Interfaces;
using SagaEventData.EventData;
using UserService.Entities;
using UserService.Repositories.Interfaces;

namespace UserService.EventHandlers.CustomerRemoveSaga;

[MarketplaceEventHandler]
public class RemoveUserHandler : IEventHandler
{
    private readonly IEventManager _eventManager;
    private readonly ICustomerRepository _customerRepository;

    public RemoveUserHandler(IEventManager eventManager, ICustomerRepository customerRepository)
    {
        _eventManager = eventManager;
        _customerRepository = customerRepository;
    }

    public async Task HandleAsync(IMarketplaceEvent marketplaceEvent)
    {
        CustomerSagaEventData sagaEventData = marketplaceEvent.EventData as CustomerSagaEventData;

        try
        {
            Customer customer = await _customerRepository.GetAsync(sagaEventData.UUID);
            await _customerRepository.DeleteAsync(customer.UUID);
            CustomerSagaEventData sagaEventDataToContinue = new CustomerSagaEventData()
            {
                TransactionUUID = sagaEventData.TransactionUUID,
                Name = customer.Name,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email,
                Surname = customer.Surname,
                UUID = customer.UUID
            };

            MarketplaceEventBase marketplaceEventBase = new MarketplaceEventBase("CustomerRemoveSaga", "CustomerRemove.Commited", sagaEventDataToContinue);
            await _eventManager.ProduceAsync(Topics.TransactionTopic, marketplaceEventBase);
        }
        catch
        {
            MarketplaceEventBase marketplaceEventBase = new MarketplaceEventBase("CustomerRemoveSaga", "CustomerRemove.Rollback", sagaEventData);
            await _eventManager.ProduceAsync(Topics.TransactionTopic, marketplaceEventBase);
        }
    }
}
