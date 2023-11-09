using MaisLocacoes.WebApi.Domain.Models.v1.Response.Address;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.Rent
{
    public class GetRentByIdResponse
    {
        public int Id { get; set; }
        public CreateClientResponse Client { get; set; }
        public string Status { get; set; }
        public decimal? Carriage { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public CreateAddressResponse Address { get; set; }
    }
}