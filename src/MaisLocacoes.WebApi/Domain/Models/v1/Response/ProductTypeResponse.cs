namespace MaisLocacoes.WebApi.Domain.Models.v1.Response
{
    public class ProductTypeResponse
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public bool IsManyParts { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}