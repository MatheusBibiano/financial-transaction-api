using Financial.Transaction.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Financial.Transaction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetStatisticsAsync(
            [FromServices] ITransactionService transactionService)
        {
            var statistics = transactionService.CalculateStatistics();

            return Ok(statistics.ToResponseObject());
        }
    }
}
