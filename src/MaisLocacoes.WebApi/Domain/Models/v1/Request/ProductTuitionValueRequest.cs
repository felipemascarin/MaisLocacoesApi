namespace MaisLocacoes.WebApi.Domain.Models.v1.Request
{
    public class ProductTuitionValueRequest
    {
        public int ProductTypeId { get; set; }
        public int QuantityPeriod { get; set; }
        public string TimePeriod { get; set; }
        public bool IsDefault { get; set; }
        public decimal Value { get; set; }
    }
}