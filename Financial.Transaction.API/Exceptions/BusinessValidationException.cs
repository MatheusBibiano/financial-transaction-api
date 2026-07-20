namespace Financial.Transaction.API.Exceptions
{
    public class BusinessValidationException : BaseApplicationException
    {
        public BusinessValidationException(string message)
            : base(message, StatusCodes.Status422UnprocessableEntity, "Erro de validação de negócio") { }
    }
}
