namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.Get
{
    public class GetProductForRentResponse
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