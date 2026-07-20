using Financial.Transaction.API.Models.Requests;
using Financial.Transaction.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Financial.Transaction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(
            [FromServices] ITransactionValidatorService transactionValidatorService,
            [FromServices] ITransactionService transactionService,
            [FromBody] RegisterTransactionRequest request)
        {
            transactionValidatorService.Validate(request);

            transactionService.Add(request.ToEntity());

            return Created();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAllAsync(
            [FromServices] ITransactionService transactionService)
        {
            transactionService.DeleteAll();

            return Ok();
        }
    }
}
