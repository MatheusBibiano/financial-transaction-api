namespace Financial.Transaction.API.Models.Entities
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public decimal Value { get; set; }
        public DateTimeOffset DateTime { get; set; }
    }
}
