namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.Get
{
    public class GetAllBillsDebtsResponse
    {
        public string ClientName { get; set; }
        public DateTime DueDate { get; set; }
        public decimal? Value { get; set; }
        public string ClientPhone { get; set; }
        public int RentId { get; set; }
        public int BillId { get; set; }
        public string BillDescription { get; set; }
        public string ProductTypeName { get; set; }
        public string ProductCode { get; set; }
        public int? Parts { get; set; }
        public DateTime? InvoiceEmittedDate { get; set; }
        public bool? IsManyParts { get; set; }
        public int? NfIdFireBase { get; set; }
    }
}
