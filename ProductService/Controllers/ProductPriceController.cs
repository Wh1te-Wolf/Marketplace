using AutoMapper;
using DataModel.DTO;
using MicroServiceBase.BaseImplementations;
using Microsoft.AspNetCore.Mvc;
using ProductService.Entities;
using ProductService.Services.Interfaces;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductPriceController : BaseCRUDController<ProductPrice, ProductPriceDTO>
    {
        public ProductPriceController(IProductPriceService baseService, IMapper mapper, ILogger<ProductPriceController> logger) : base(baseService, mapper, logger)
        {

        }
    }
}
