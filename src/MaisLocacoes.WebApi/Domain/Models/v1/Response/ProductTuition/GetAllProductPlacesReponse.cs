namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.ProductTuition
{
    public class GetAllProductPlacesReponse
    {
        public string Type { get; set; } //productType
        public bool IsManyParts { get; set; } //productType
        public string Code { get; set; } //product
        public int Parts { get; set; } //productTuition e product
        public double? Latitude { get; set; } //rentedPlace
        public double? Longitude { get; set; } //rentedPlace
    }
}
