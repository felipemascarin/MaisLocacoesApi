namespace MaisLocacoes.WebApi.Domain.Models.v1.Response
{
    public class CreateContractResponse
    {
        public int Id { get; set; }
        public Guid GuidId { get; set; }
        public int RentId { get; set; }
        public int? ProductQuantity { get; set; }
        public string? UrlSignature { get; set; }
        public DateTime SignedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
