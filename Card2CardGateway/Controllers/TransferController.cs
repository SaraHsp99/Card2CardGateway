using Card2CardGateway.Application.UseCases.Transfer.DTO;
using Card2CardGateway.Application.UseCases.Transfer.Interfaces;
using Card2CardGateway.Application.UseCases.Transfer.Services;
using Microsoft.AspNetCore.Mvc;

namespace Card2CardGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransferController : ControllerBase
    {
        private readonly ITransactionService _service;

        public TransferController(TransactionService service) => _service = service;

        [HttpPost]
        public async Task<IActionResult> Transfer([FromBody] TransferRequestDto request)
        {
            var response = await _service.TransferAsync(request);
            return Ok(response);
        }

        [HttpGet("inquiry/{requestTraceId}")]
        public async Task<IActionResult> Inquiry(Guid requestTraceId)
        {
            var response = await _service.InquiryAsync(requestTraceId);
            return Ok(response);
        }
    }

}
