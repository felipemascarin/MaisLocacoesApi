namespace MaisLocacoes.WebApi.Domain.Models.v1.Request
{
    public class RentRequest
    {
        public int ClientId { get; set; }
        public decimal? Carriage { get; set; }
        public DateTime? FirstDueDate { get; set; }
        public virtual AddressRequest Address { get; set; }
    }
}