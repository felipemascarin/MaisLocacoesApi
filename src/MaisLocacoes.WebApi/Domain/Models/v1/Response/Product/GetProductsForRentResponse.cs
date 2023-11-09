namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.Product
{
    public class GetProductsForRentResponse
    {
        public string Code { get; set; }
        public int FreeParts { get; set; }

        public class GetProductForRentDtoResponse
        {
            public string Code { get; set; }
            public int Parts { get; set; }
            public int RentedParts { get; set; }
        }
    }
}