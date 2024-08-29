using Core.BaseImplementations;
using Events;
using Events.Services.Interfaces;
using RemoteRESTClients.Interfaces;
using SagaEventData.EventData;
using System;
using UserService.Entities;
using UserService.Repositories.Interfaces;
using UserService.Services.Interfaces;

namespace UserService.Services
{
    public class CustomerService : BaseService<Customer>, ICustomerService
    {
        private readonly IEventManager _eventManager;
        private readonly ISagaRemoteProcessManager _sagaRemoteProcessManager;

        public CustomerService(ICustomerRepository customerRepository, IEventManager eventManager, ISagaRemoteProcessManager sagaRemoteProcessManager) : base(customerRepository)
        {
            _eventManager = eventManager;
            _sagaRemoteProcessManager = sagaRemoteProcessManager;
        }

        public override async Task<Customer> AddAsync(Customer entity)
        {
            CustomerSagaEventData customerSagaEventData = new CustomerSagaEventData() 
            {
                UUID = entity.UUID,
                Email = entity.Email,
                Name = entity.Name,
                PhoneNumber = entity.PhoneNumber,
                Surname = entity.Surname
            };
            bool result = await _sagaRemoteProcessManager.ProcessAsync("CustomerCreateSaga", customerSagaEventData);

            if (result) 
                return entity;
            else
                throw new Exception("Customer create error");
        }

        public override async Task DeleteAsync(Guid uuid)
        {
            CustomerSagaEventData customerSagaEventData = new CustomerSagaEventData(uuid);
            bool result = await _sagaRemoteProcessManager.ProcessAsync("CustomerRemoveSaga", customerSagaEventData);

            if (!result)
                throw new Exception("Customer remove error");
        }
    }
}
