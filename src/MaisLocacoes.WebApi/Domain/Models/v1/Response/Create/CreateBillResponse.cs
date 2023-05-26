namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.Create
{
    public class CreateBillResponse
    {
        public int Id { get; set; }
        public int RentId { get; set; }
        public decimal Value { get; set; }
        public DateTime? PayDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
        public int? NfIdFireBase { get; set; }
        public string? PaymentMode { get; set; }
    }
}