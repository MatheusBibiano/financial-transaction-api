using Financial.Transaction.API.Services;
using Financial.Transaction.API.Settings;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Financial.Transaction.Tests
{
    public class TransactionServiceTests
    {
        private readonly Mock<ILogger<TransactionService>> _loggerMock;
        private readonly StatisticsSettings _settings;
        private readonly IOptions<StatisticsSettings> _optionsSettings;
        private readonly TransactionService _service;

        public TransactionServiceTests()
        {
            _loggerMock = new Mock<ILogger<TransactionService>>();
            _settings = new StatisticsSettings { WindowInSeconds = 60 };
            _optionsSettings = Options.Create(_settings);

            _service = new TransactionService(_optionsSettings, _loggerMock.Object);
        }

        [Fact]
        public void Add_ShouldConvertDateTimeToUtcAndStoreTransaction()
        {
            // Arrange
            var localTime = DateTime.Now;
            var transaction = new API.Models.Entities.Transaction
            {
                Value = 100m,
                DateTime = localTime
            };

            // Act
            _service.Add(transaction);

            // Assert
            var stats = _service.CalculateStatistics();
            stats.Count.Should().Be(1);
            transaction.DateTime.Offset.Should().Be(TimeSpan.Zero);
        }

        [Fact]
        public void CalculateStatistics_WhenNoTransactionsExist_ShouldReturnZeroes()
        {
            // Act
            var stats = _service.CalculateStatistics();

            // Assert
            stats.Count.Should().Be(0);
            stats.Sum.Should().Be(0m);
            stats.Avg.Should().Be(0m);
            stats.Min.Should().Be(0m);
            stats.Max.Should().Be(0m);
        }

        [Fact]
        public void CalculateStatistics_WithValidRecentTransactions_ShouldCalculateCorrectly()
        {
            // Arrange
            var now = DateTimeOffset.UtcNow;
            _service.Add(new API.Models.Entities.Transaction { Value = 10m, DateTime = now.AddSeconds(-5).DateTime });
            _service.Add(new API.Models.Entities.Transaction { Value = 20m, DateTime = now.AddSeconds(-10).DateTime });
            _service.Add(new API.Models.Entities.Transaction { Value = 30m, DateTime = now.AddSeconds(-15).DateTime });

            // Act
            var stats = _service.CalculateStatistics();

            // Assert
            stats.Count.Should().Be(3);
            stats.Sum.Should().Be(60m);
            stats.Avg.Should().Be(20m);
            stats.Min.Should().Be(10m);
            stats.Max.Should().Be(30m);
        }

        [Fact]
        public void CalculateStatistics_ShouldIgnoreTransactionsOutsideTimeWindow()
        {
            // Arrange
            var now = DateTime.UtcNow;

            // Dentro da janela (60s)
            _service.Add(new API.Models.Entities.Transaction { Value = 50m, DateTime = now.AddSeconds(-10) });

            // Fora da janela (mais antiga que 60s)
            _service.Add(new API.Models.Entities.Transaction { Value = 100m, DateTime = now.AddSeconds(-61) });

            // Act
            var stats = _service.CalculateStatistics();

            // Assert
            stats.Count.Should().Be(1);
            stats.Sum.Should().Be(50m);
            stats.Min.Should().Be(50m);
            stats.Max.Should().Be(50m);
        }

        [Fact]
        public void DeleteAll_ShouldRemoveAllTransactions()
        {
            // Arrange
            _service.Add(new API.Models.Entities.Transaction { Value = 100m, DateTime = DateTime.UtcNow });
            _service.Add(new API.Models.Entities.Transaction { Value = 200m, DateTime = DateTime.UtcNow });

            // Act
            _service.DeleteAll();

            // Assert
            var stats = _service.CalculateStatistics();
            stats.Count.Should().Be(0);
        }
    }
}