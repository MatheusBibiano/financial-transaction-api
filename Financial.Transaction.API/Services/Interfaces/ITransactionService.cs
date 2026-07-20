using Financial.Transaction.API.Models.Entities;
using Financial.Transaction.API.Models.Response;

namespace Financial.Transaction.API.Services.Interfaces
{
    public interface ITransactionService
    {
        void Add(Models.Entities.Transaction transaction);

        void DeleteAll();

        Statistics CalculateStatistics();
    }
}
