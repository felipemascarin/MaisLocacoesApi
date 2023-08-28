namespace MaisLocacoes.WebApi.Domain.Models.v1.Response
{
    public class ProductTuitionValueResponse
    {
        public int Id { get; set; }
        public int ProductTypeId { get; set; }
        public int QuantityPeriod { get; set; }
        public string TimePeriod { get; set; }
        public bool IsDefault { get; set; }
        public decimal Value { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}