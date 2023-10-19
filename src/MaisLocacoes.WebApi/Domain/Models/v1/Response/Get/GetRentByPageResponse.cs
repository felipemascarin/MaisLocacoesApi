namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.Get
{
    public class GetRentByPageResponse
    {
        public int Id { get; set; }
        public ClientResponse Client { get; set; }
        public AddressResponse Address { get; set; }
        public List<string> ProductCodes { get; set; } = new List<string>() { };
        public string Status { get; set; }
        public decimal? Carriage { get; set; }
        public string Description { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime FinalDate { get; set; }
        public DateTime? SignedAt { get; set; }
        public string? UrlSignature { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
