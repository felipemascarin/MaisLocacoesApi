namespace MaisLocacoes.WebApi.Domain.Models.v1.Request
{
    public class ProductWasteRequest
    {
        public int ProductId { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }

    }
}