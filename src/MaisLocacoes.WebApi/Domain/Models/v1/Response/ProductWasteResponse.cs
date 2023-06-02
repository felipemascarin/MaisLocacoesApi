namespace MaisLocacoes.WebApi.Domain.Models.v1.Response
{
    public class ProductWasteResponse
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}