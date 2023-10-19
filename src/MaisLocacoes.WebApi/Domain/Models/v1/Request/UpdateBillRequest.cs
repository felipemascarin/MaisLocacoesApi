namespace MaisLocacoes.WebApi.Domain.Models.v1.Request
{
    public class UpdateBillRequest
    {
        public int RentId { get; set; }
        public int? ProductTuitionId { get; set; }
        public int? Order { get; set; }
        public decimal Value { get; set; }
        public DateTime? PayDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? InvoiceEmittedDate { get; set; }
        public int? NfIdFireBase { get; set; }
        public string? PaymentMode { get; set; }
        public string? Description { get; set; }
    }
}
