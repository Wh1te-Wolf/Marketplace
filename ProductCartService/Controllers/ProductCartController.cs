using AutoMapper;
using MicroServiceBase.BaseImplementations;
using Microsoft.AspNetCore.Mvc;
using ProductCartService.DTO;
using ProductCartService.Entities;
using ProductCartService.Services.Interfaces;

namespace ProductCartService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCartController : BaseCRUDController<ProductCart, ProductCartDTO>
    {
        public ProductCartController(IProductCartService baseService, IMapper mapper, ILogger<ProductCartController> logger) : base(baseService, mapper, logger)
        {

        }
    }
}
