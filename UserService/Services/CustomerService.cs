using Core.BaseImplementations;
using Events;
using Events.Services.Interfaces;
using UserService.Entities;
using UserService.Repositories.Interfaces;
using UserService.Services.Interfaces;

namespace UserService.Services
{
    public class CustomerService : BaseService<Customer>, ICustomerService
    {
        private readonly IEventManager _eventManager;

        public CustomerService(ICustomerRepository customerRepository, IEventManager eventManager) : base(customerRepository)
        {
            _eventManager = eventManager;
        }

        public override async Task<Customer> AddAsync(Customer entity)
        {
            Customer customer = await base.AddAsync(entity);

            await _eventManager.ProduceAsync(Topics.MainTopic, new CustomerCreatedEvent(customer.UUID));

            return customer;
        }

        public override async Task DeleteAsync(Guid uuid)
        {
            await base.DeleteAsync(uuid);

            await _eventManager.ProduceAsync(Topics.MainTopic, new CustomerRemovedEvent(uuid));

            return;
        }
    }
}
