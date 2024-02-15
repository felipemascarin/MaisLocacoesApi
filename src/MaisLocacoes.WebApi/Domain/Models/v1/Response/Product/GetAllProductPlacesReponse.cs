namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.Product
{
    public class GetAllProductPlacesReponse
    {
        public List<ProductPlace> RentedProducts { get; set; } = new List<ProductPlace>();
        public List<ProductPlace> FreeProducts { get; set; } = new List<ProductPlace>();

        public class ProductPlace
        {
            public string Type { get; set; } //productType
            public bool IsManyParts { get; set; } //productType
            public string Code { get; set; } //product
            public int Parts { get; set; } //product
            public string Status { get; set; } //product
            public double? Latitude { get; set; } //rentedPlace
            public double? Longitude { get; set; } //rentedPlace
        }
    }
}
