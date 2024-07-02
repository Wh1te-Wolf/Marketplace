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
    public class BillController : BaseCRUDController<Bill, BillDTO>
    {
        private readonly ILogger<BillController> _logger;
        private readonly IMapper _mapper;
        private readonly IBillService _billService;

        public BillController(IBillService baseService, IMapper mapper, ILogger<BillController> logger) : base(baseService, mapper, logger)
        {
            _logger = logger;
            _mapper = mapper;
            _billService = baseService;
        }

        [HttpPost(nameof(BuildBill))]
        public async Task<IActionResult> BuildBill([FromQuery] Guid productCartUUID)
        {
            try
            {
                Bill bill = await _billService.BuildBillAsync(productCartUUID);
                BillDTO billDTO = _mapper.Map<BillDTO>(bill);
                return Ok(billDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
