using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.Create
{
    public class CreateRentResponse
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Status { get; set; }
        public decimal? Carriage { get; set; }
        public CreateAddressResponse Address { get; set; }
    }
}