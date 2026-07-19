using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Financial.Transaction.API.Models.Requests
{
    public class RegisterTransactionRequest
    {
        [JsonPropertyName("valor")]
        [Required(ErrorMessage = "O campo 'valor' é obrigatório.")]
        [Range(0.0, double.MaxValue, ErrorMessage = $"O campo 'valor' deve ser maior ou igual a zero.")]
        public decimal? Value { get; set; }

        [JsonPropertyName("dataHora")]
        [Required(ErrorMessage = $"O campo 'dataHora' é obrigatório.")]
        public DateTimeOffset? DateTime { get; set; }

        public Entities.Transaction ToEntity()
        {
            return new Entities.Transaction
            {
                Id = Guid.NewGuid(),
                Value = Value,
                DateTime = DateTime
            };
        }
    }
}
