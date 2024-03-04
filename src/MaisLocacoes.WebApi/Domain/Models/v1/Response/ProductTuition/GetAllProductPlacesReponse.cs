namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.ProductTuition
{
    public class GetAllProductPlacesReponse
    {
        public string Type { get; set; }
        public bool IsManyParts { get; set; }
        public string Code { get; set; }
        public int Parts { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
