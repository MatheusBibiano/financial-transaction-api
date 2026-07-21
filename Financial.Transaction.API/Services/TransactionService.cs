using Financial.Transaction.API.Models.Entities;
using Financial.Transaction.API.Services.Interfaces;
using Financial.Transaction.API.Settings;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace Financial.Transaction.API.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly StatisticsSettings _settings;
        private ConcurrentBag<Models.Entities.Transaction> _transactions = new();

        public TransactionService(IOptions<StatisticsSettings> statisticsSettings)
        {
            _settings = statisticsSettings.Value;
        }

        public void Add(Models.Entities.Transaction transaction)
        {
            transaction.DateTime = transaction.DateTime.ToUniversalTime();
            _transactions.Add(transaction);
        }

        public void DeleteAll() => Interlocked.Exchange(ref _transactions, new ConcurrentBag<Models.Entities.Transaction>());

        public Statistics CalculateStatistics()
        {
            var timeLimit = DateTimeOffset.UtcNow.AddSeconds(-_settings.WindowInSeconds);

            var recentTransactions = _transactions
                .Where(transaction => transaction.DateTime >= timeLimit)
                .ToList();

            if (recentTransactions.Count == 0)
                return new Statistics(0L, 0m, 0m, 0m, 0m);

            return new Statistics(
                (long)recentTransactions.Count,
                recentTransactions.Sum(t => t.Value),
                recentTransactions.Average(t => t.Value),
                recentTransactions.Min(t => t.Value),
                recentTransactions.Max(t => t.Value)
            );
        }
    }
}
