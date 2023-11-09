namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.ProductWaste
{
    public class GetAllProductWastesByProductIdResponse
    {
        public List<CreateProductWasteResponse> ProductsWastes { get; set; }
        public decimal TotalWastesValue { get; set; }
    }
}
