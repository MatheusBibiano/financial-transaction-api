using Financial.Transaction.API.Services.Interfaces;
using System.Collections.Concurrent;

namespace Financial.Transaction.API.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ConcurrentBag<Models.Entities.Transaction> _transactions = [];

        public void Add(Models.Entities.Transaction transaction) => _transactions.Add(transaction);
    }
}
