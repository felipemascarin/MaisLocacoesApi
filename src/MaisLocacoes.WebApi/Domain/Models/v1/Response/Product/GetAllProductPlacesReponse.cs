namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.Product
{
    public class GetAllProductPlacesReponse
    {
        public List<ProductPlace> RentedProducts { get; set; } = new List<ProductPlace>();
        public List<ProductPlace> FreeProducts { get; set; } = new List<ProductPlace>();

        public class ProductPlace
        {
            public string Type { get; set; }
            public bool IsManyParts { get; set; }
            public string Code { get; set; }
            public int Parts { get; set; }
            public string Status { get; set; }
            public double Latitude { get; set; } = 0;
            public double Longitude { get; set; } = 0;
        }
    }
}
