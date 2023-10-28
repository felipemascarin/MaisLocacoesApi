namespace MaisLocacoes.WebApi.Domain.Models.v1.Request
{
    public class CreateRentRequest
    {
        public int ClientId { get; set; }
        public decimal? Carriage { get; set; }
        public string Description { get; set; }
        public virtual CreateAddressRequest Address { get; set; }
    }
}