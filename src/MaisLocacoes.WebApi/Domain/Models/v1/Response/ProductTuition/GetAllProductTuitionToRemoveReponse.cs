using MaisLocacoes.WebApi.Domain.Models.v1.Response.Rent;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.ProductTuition
{
    public class GetAllProductTuitionToRemoveReponse
    {
        public int Id { get; set; }
        public GetRentClientResponse Rent { get; set; }
        public CreateProductTypeResponse ProductType { get; set; }
        public int? ProductId { get; set; }
        public string ProductCode { get; set; }
        public decimal Value { get; set; }
        public DateTime InitialDateTime { get; set; }
        public DateTime FinalDateTime { get; set; }
        public string Status { get; set; }
        public DateTime? FirstDueDate { get; set; }
        public int QuantityPeriod { get; set; }
        public string TimePeriod { get; set; }
        public int Parts { get; set; }
        public bool IsEditable { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
