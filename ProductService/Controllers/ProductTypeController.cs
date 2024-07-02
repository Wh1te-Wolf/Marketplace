using AutoMapper;
using MicroServiceBase.BaseImplementations;
using Microsoft.AspNetCore.Mvc;
using ProductService.DTO;
using ProductService.Entities;
using ProductService.Services.Interfaces;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypeController : BaseCRUDController<ProductType, ProductTypeDTO>
    {
        public ProductTypeController(IProductTypeService baseService, IMapper mapper, ILogger<ProductTypeController> logger) : base(baseService, mapper, logger)
        {

        }
    }
}
