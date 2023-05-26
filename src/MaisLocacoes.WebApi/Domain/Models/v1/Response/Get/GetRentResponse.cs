namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.Get
{
    public class GetRentResponse
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Status { get; set; }
        public decimal? Carriage { get; set; }
        public GetAddressResponse Address { get; set; }
    }
}
