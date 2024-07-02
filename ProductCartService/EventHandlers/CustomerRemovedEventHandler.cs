using Events.Attributes;
using Events.Data;
using Events.Entities;
using Events.Handlers;
using ProductCartService.Entities;
using ProductCartService.Services.Interfaces;

namespace ProductCartService.EventHandlers
{
    [MarketplaceEventHandler]
    public class CustomerRemovedEventHandler : IEventHandler
    {
        private readonly IProductCartService _productCartService;

        public CustomerRemovedEventHandler(IProductCartService productCartService)
        {
            _productCartService = productCartService;
        }

        public async Task HandleAsync(IMarketplaceEvent marketplaceEvent)
        {
            CustomerCreatedEventData data = marketplaceEvent.EventData as CustomerCreatedEventData;

            ProductCart productCart = new ProductCart()
            {
                CustomerUUID = data.CustomerUUID,
                Items = new List<ProductCartItem>()
            };

            await _productCartService.DeleteAsync(productCart.CustomerUUID);
        }
    }
}
