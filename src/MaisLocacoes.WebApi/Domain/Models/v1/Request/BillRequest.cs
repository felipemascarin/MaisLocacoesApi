namespace MaisLocacoes.WebApi.Domain.Models.v1.Request
{
    public class BillRequest
    {
        public int ProductTuitionId { get; set; }
        public decimal Value { get; set; }
        public DateTime? PayDate { get; set; }
        public DateTime DueDate { get; set; }
        public int? NfIdFireBase { get; set; }
        public string? PaymentMode { get; set; }
    }
}