namespace MaisLocacoes.WebApi.Domain.Models.v1.Request
{
    public class CreateContractRequest
    {
        public int? RentId { get; set; }
        public string UrlSignature { get; set; }
        public DateTime? SignedAt { get; set; }
    }
}
