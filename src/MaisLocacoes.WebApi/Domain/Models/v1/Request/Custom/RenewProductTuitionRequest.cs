namespace MaisLocacoes.WebApi.Domain.Models.v1.Request.Custom
{
    public class RenewProductTuitionRequest
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public DateTime? FirstDueDate { get; set; }
        public int QuantityPeriod { get; set; }
        public string TimePeriod { get; set; }
        public DateTime InitialDateTime { get; set; }
        public DateTime FinalDateTime { get; set; }
    }
}