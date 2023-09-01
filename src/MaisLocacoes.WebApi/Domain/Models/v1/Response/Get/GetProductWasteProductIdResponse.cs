namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.Get
{
    public class GetProductWasteProductIdResponse
    {
        public List<ProductWasteResponse> ProductsWastes { get; set; }
        public decimal TotalWastesValue { get; set; }
    }
}
