namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.Get
{
    public class GetProductResponse
    {
        public string Code { get; set; }
        public string ProductType { get; set; }
        public int? SupplierId { get; set; }
        public string? Description { get; set; }
        public DateTime? DateBought { get; set; }
        public decimal? BoughtValue { get; set; }
        public string Status { get; set; }
        public int? CurrentRentedPlaceId { get; set; }
        public int Parts { get; set; }
        public int RentedParts { get; set; }
    }
}
