using Financial.Transaction.API.Exceptions;
using Financial.Transaction.API.Models.Requests;
using Financial.Transaction.API.Services.Interfaces;

namespace Financial.Transaction.API.Services
{
    public class TransactionValidatorService : ITransactionValidatorService
    {
        public void Validate(RegisterTransactionRequest request)
        {
            var dateTime = request.DateTime!.Value;

            if (!OccurredInPast(dateTime))
                throw new BusinessValidationException("A data da transação deve ser no passado.");
        }

        private static bool OccurredInPast(DateTimeOffset transactionDateTime) => transactionDateTime < DateTimeOffset.UtcNow;
    }
}
