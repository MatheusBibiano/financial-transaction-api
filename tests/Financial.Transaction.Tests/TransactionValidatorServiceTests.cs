using Financial.Transaction.API.Exceptions;
using Financial.Transaction.API.Models.Requests;
using Financial.Transaction.API.Services;
using FluentAssertions;

namespace Financial.Transaction.Tests
{
    public class TransactionValidatorServiceTests
    {
        private readonly TransactionValidatorService _validatorService;

        public TransactionValidatorServiceTests()
        {
            _validatorService = new TransactionValidatorService();
        }

        [Fact]
        public void Validate_WhenTransactionIsInThePast_ShouldNotThrowException()
        {
            // Arrange
            var request = new RegisterTransactionRequest
            {
                Value = 100.00m,
                DateTime = DateTimeOffset.UtcNow.AddMinutes(-5) // Transação há 5 minutos no passado
            };

            // Act
            Action act = () => _validatorService.Validate(request);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Validate_WhenTransactionIsInTheFuture_ShouldThrowBusinessValidationException()
        {
            // Arrange
            var request = new RegisterTransactionRequest
            {
                Value = 100.00m,
                DateTime = DateTimeOffset.UtcNow.AddMinutes(5) // Transação 5 minutos no futuro
            };

            // Act
            Action act = () => _validatorService.Validate(request);

            // Assert
            act.Should()
               .Throw<BusinessValidationException>()
               .WithMessage("A data da transação deve ser no passado.");
        }

        [Fact]
        public void Validate_WhenTransactionIsExactlyNow_ShouldThrowBusinessValidationException()
        {
            // Arrange
            // Adiciona um pequeno buffer no futuro para garantir que o tempo do UtcNow na validação 
            // seja menor do que o recebido no request
            var request = new RegisterTransactionRequest
            {
                Value = 50.00m,
                DateTime = DateTimeOffset.UtcNow.AddMilliseconds(100)
            };

            // Act
            Action act = () => _validatorService.Validate(request);

            // Assert
            act.Should().Throw<BusinessValidationException>();
        }
    }
}
