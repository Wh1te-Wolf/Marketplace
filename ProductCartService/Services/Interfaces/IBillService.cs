using Core.Interfaces;
using ProductCartService.Entities;

namespace ProductCartService.Services.Interfaces
{
    public interface IBillService : IBaseService<Bill>
    {
        Task<Bill> BuildBillAsync(Guid productCartUUID);
    }
}
