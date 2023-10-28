namespace MaisLocacoes.WebApi.Domain.Models.v1.Request
{
    public class UpdateProductRequest
    {
        public string Code { get; set; }
        public int ProductTypeId { get; set; }
        public int? SupplierId { get; set; }
        public string Description { get; set; }
        public DateTime? DateBought { get; set; }
        public decimal? BoughtValue { get; set; }
        public int? CurrentRentedPlaceId { get; set; }
        public int Parts { get; set; }
        public int RentedParts { get; set; }
    }
}
