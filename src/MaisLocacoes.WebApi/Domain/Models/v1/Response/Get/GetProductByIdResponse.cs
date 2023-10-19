namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.Get
{
    public class GetProductByIdResponse
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int? SupplierId { get; set; }
        public string Description { get; set; }
        public DateTime? DateBought { get; set; }
        public decimal? BoughtValue { get; set; }
        public string Status { get; set; }
        public int? CurrentRentedPlaceId { get; set; }
        public int Parts { get; set; }
        public int RentedParts { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public CreateProductTypeResponse ProductType { get; set; }
    }
}