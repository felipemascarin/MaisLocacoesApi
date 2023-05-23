namespace MaisLocacoes.WebApi.Domain.Models.v1.Request
{
    public class ProductWasteRequest
    {
        public string ProductCode { get; set; }
        public string ProductType { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }

    }
}