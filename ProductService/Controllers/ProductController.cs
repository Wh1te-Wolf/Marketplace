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
    public class ProductController : BaseCRUDController<Product, ProductDTO>
    {
        public ProductController(IProductService baseService, IMapper mapper, ILogger<ProductController> logger) : base(baseService, mapper, logger)
        {

        }
    }
}
