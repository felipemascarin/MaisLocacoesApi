using MaisLocacoes.WebApi.Domain.Models.v1.Response.ProductType;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.Product
{
    public class GetProductsByPageResponse
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
        public ProductTypeResponse ProductType { get; set; }

        public class ProductTypeResponse
        {
            public int Id { get; set; }
            public string Type { get; set; }
            public bool IsManyParts { get; set; }
            public string CreatedBy { get; set; }
            public string LastCreatedCode { get; set; }
            public DateTime CreatedAt { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime? UpdatedAt { get; set; }
        }
    }
}