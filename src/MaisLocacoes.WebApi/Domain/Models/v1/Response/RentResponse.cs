using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Response
{
    public class RentResponse
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Status { get; set; }
        public decimal? Carriage { get; set; }
        public DateTime FirstDueDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public AddressResponse Address { get; set; }
    }
}