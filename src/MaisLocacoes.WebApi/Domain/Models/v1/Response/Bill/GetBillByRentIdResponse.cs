using MaisLocacoes.WebApi.Domain.Models.v1.Response.ProductType;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.Bill
{
    public class GetBillByRentIdResponse
    {
        public int Id { get; set; }
        public int RentId { get; set; }
        public int? ProductTuitionId { get; set; }
        public string ProductCode { get; set; }
        public int? ProductTuitionParts { get; set; }
        public CreateProductTypeResponse ProductType { get; set; }
        public decimal Value { get; set; }
        public int? Order { get; set; }
        public DateTime? PayDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? InvoiceEmittedDate { get; set; }
        public string Status { get; set; }
        public int? NfIdFireBase { get; set; }
        public string PaymentMode { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}