namespace MaisLocacoes.WebApi.Domain.Models.v1.Request
{
    public class UpdateRentedPlaceRequest
    {
        public int ProductId { get; set; }
        public int? RentId { get; set; }
        public int? QgId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public int ProductParts { get; set; }
    }
}