using Core.Entities;
using Events;
using Events.Attributes;
using Events.Entities;
using Events.Handlers;
using Events.Services.Interfaces;
using ProductCartService.Entities;
using ProductCartService.Repositories.Interfaces;
using SagaEventData.EventData;

namespace ProductCartService.EventHandlers.CustomerRemoveSaga
{
    [MarketplaceEventHandler]
    public class RemoveProductCardHandler : IEventHandler
    {
        private readonly IProductCartRepository _productCartRepository;
        private readonly IEventManager _eventManager;

        public RemoveProductCardHandler(IProductCartRepository productCartRepository, IEventManager eventManager)
        {
            _productCartRepository = productCartRepository;
            _eventManager = eventManager;
        }

        public async Task HandleAsync(IMarketplaceEvent marketplaceEvent)
        {
            CustomerSagaEventData sagaEventData = marketplaceEvent.EventData as CustomerSagaEventData;

            try
            {
                Filter filter = new Filter();
                filter.Equals(nameof(ProductCart.CustomerUUID), sagaEventData.UUID);
                ProductCart? productCart = (await _productCartRepository.FindAsync(filter)).FirstOrDefault();

                await _productCartRepository.DeleteAsync(productCart.UUID);

                MarketplaceEventBase marketplaceEventBase = new MarketplaceEventBase("CustomerRemoveSaga", "ProductCartRemove.Commited", sagaEventData);
                await _eventManager.ProduceAsync(Topics.TransactionTopic, marketplaceEventBase);
            }
            catch
            {
                MarketplaceEventBase marketplaceEventBase = new MarketplaceEventBase("CustomerRemoveSaga", "CustomerRemove.Rollback", sagaEventData);
                await _eventManager.ProduceAsync(Topics.TransactionTopic, marketplaceEventBase);
            }
        }
    }
}
