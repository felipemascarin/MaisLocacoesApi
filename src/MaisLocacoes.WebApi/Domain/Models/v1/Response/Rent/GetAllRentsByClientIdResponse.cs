namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.Rent
{
    public class GetAllRentsByClientIdResponse
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
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