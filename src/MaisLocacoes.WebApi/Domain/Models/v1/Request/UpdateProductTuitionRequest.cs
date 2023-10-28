namespace MaisLocacoes.WebApi.Domain.Models.v1.Request
{
    public class UpdateProductTuitionRequest
    {
        public int RentId { get; set; }
        public int ProductTypeId { get; set; }
        public string ProductCode { get; set; }
        public decimal Value { get; set; }
        public int Parts { get; set; }
        public string Status { get; set; }
        public DateTime? FirstDueDate { get; set; }
        public int QuantityPeriod { get; set; }
        public string TimePeriod { get; set; }
        public DateTime? InitialDateTime { get; set; }
        public DateTime? FinalDateTime { get; set; }
    }
}