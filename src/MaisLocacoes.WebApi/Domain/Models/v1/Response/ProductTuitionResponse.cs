namespace MaisLocacoes.WebApi.Domain.Models.v1.Response
{
    public class ProductTuitionResponse
    {
        public int Id { get; set; }
        public int RentId { get; set; }
        public string ProductType { get; set; }
        public string ProductCode { get; set; }
        public decimal Value { get; set; }
        public DateTime InitialDateTime { get; set; }
        public DateTime FinalDateTime { get; set; }
        public int Parts { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}