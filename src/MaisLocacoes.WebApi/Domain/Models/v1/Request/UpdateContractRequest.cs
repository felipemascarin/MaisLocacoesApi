namespace MaisLocacoes.WebApi.Domain.Models.v1.Request
{
    public class UpdateContractRequest
    {
        public int? RentId { get; set; }
        public int? ProductQuantity { get; set; }
        public string UrlSignature { get; set; }
        public DateTime? SignedAt { get; set; }
    }
}
