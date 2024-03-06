namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.Product
{
    public class GetAllProductPlacesResponse
    {
        public List<PinResponse> RentedProducts { get; set; } = new List<PinResponse>();
        public List<PinResponse> FreeProducts { get; set; } = new List<PinResponse>();

        public class PinResponse
        {
            public double? Latitude { get; set; }
            public double? Longitude { get; set; }
            public List<ProductsPinResponse> Products { get; set; } = new List<ProductsPinResponse>();
        }

        public class ProductsPinResponse
        {
            public string Type { get; set; }
            public bool IsManyParts { get; set; }
            public string Code { get; set; }
            public int Parts { get; set; }
            public string Status { get; set; }
        }
    }
}
