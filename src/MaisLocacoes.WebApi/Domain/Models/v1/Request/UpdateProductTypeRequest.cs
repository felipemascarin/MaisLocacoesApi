namespace MaisLocacoes.WebApi.Domain.Models.v1.Request
{
    public class UpdateProductTypeRequest
    {
        public string Type { get; set; }
        public bool IsManyParts { get; set; }
    }
}
