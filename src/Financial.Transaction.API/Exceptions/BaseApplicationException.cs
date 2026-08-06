namespace Financial.Transaction.API.Exceptions
{
    public class BaseApplicationException : Exception
    {
        public int StatusCode { get; }
        public string Title { get; }

        protected BaseApplicationException(string message, int statusCode, string title) : base(message)
        {
            StatusCode = statusCode;
            Title = title;
        }
    }
}
