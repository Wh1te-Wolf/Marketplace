using Core.Entities;
using Events;
using Events.Attributes;
using Events.Entities;
using Events.Handlers;
using Events.Services.Interfaces;
using ProductCartService.Entities;
using ProductCartService.Repositories.Interfaces;
using SagaEventData.EventData;

namespace ProductCartService.EventHandlers.CustomerCreateSaga
{
    [MarketplaceEventHandler]
    public class CreateProductCartHandler : IEventHandler
    {
        private readonly IProductCartRepository _productCartRepository;
        private readonly IEventManager _eventManager;

        public CreateProductCartHandler(IProductCartRepository productCartRepository, IEventManager eventManager)
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

                await _productCartRepository.AddAsync(productCart);

                MarketplaceEventBase marketplaceEventBase = new MarketplaceEventBase("CustomerCreateSaga", "ProductCartCreate._commited", sagaEventData);
                await _eventManager.ProduceAsync(Topics.TransactionTopic, marketplaceEventBase);
            }
            catch
            {
                MarketplaceEventBase marketplaceEventBase = new MarketplaceEventBase("CustomerCreateSaga", "ProductCartCreate._rollback", sagaEventData);
                await _eventManager.ProduceAsync(Topics.TransactionTopic, marketplaceEventBase);
            }
        }
    }
}
