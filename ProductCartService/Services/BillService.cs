using Core.BaseImplementations;
using DataModel.DTO;
using ProductCartService.Entities;
using ProductCartService.Entities.Enum;
using ProductCartService.Repositories.Interfaces;
using ProductCartService.Services.Interfaces;
using RemoteRESTClients.Interfaces;

namespace ProductCartService.Services
{
    public class BillService : BaseService<Bill>, IBillService
    {
        private readonly IProductCartService _productCartService;
        private readonly IProductRemoteService _productRemoteService;

        public BillService(IBillRepository baseRepository, IProductCartService productCartService, IProductRemoteService productRemoteService) : base(baseRepository)
        {
            _productCartService = productCartService;
            _productRemoteService = productRemoteService;
        }

        public async Task<Bill> BuildBillAsync(Guid productCartUUID)
        {
            ProductCart productCart = await _productCartService.GetAsync(productCartUUID);
            Bill bill = new Bill()
            {
                CustomerUUID = productCart.CustomerUUID,
                DateTime = DateTime.UtcNow,
                PaymentStatus = PaymentStatus.Awaiting,
                Items = new List<BillItem>() 
            };

            foreach (ProductCartItem item in productCart.Items) 
            {
                ProductDTO productDTO = await _productRemoteService.GetAsync(item.ProductUUID);
                ProductPriceDTO price = productDTO.Prices.FirstOrDefault(p => p.IsLastPrice)?? productDTO.Prices.LastOrDefault();
                BillItem billItem = new BillItem()
                {
                    ProductUUID = item.ProductUUID,
                    Quantity = item.Quantity,
                    ProductPriceUUID = price.UUID,
                    Price = price.Value * item.Quantity
                };

                bill.Items.Add(billItem);
            }

            bill.Price = bill.Items.Sum(x => x.Price);
            Bill savedBill = await AddAsync(bill);
            return savedBill;
        }
    }
}
