using AutoMapper;
using Core.Interfaces;
using MicroServiceBase.BaseImplementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.DTO;
using UserService.Entities;
using UserService.Services.Interfaces;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : BaseCRUDController<Customer, CustomerDTO>
    {
        public CustomerController(ICustomerService customerService, IMapper mapper, ILogger<CustomerController> logger) 
            : base(customerService, mapper, logger)
        {

        }
    }
}
