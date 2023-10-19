namespace MaisLocacoes.WebApi.Domain.Models.v1.Response
{
    public class CreateQgResponse
    {
        public int Id { get; set; }
        public int AddressId { get; set; }
        public string Description { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}