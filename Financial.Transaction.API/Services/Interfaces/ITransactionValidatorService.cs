using Financial.Transaction.API.Models.Requests;

namespace Financial.Transaction.API.Services.Interfaces
{
    public interface ITransactionValidatorService
    {
        void Validate(RegisterTransactionRequest request);
    }
}
