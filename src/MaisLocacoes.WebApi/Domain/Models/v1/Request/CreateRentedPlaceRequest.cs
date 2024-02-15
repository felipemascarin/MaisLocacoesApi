namespace MaisLocacoes.WebApi.Domain.Models.v1.Request
{
    public class CreateRentedPlaceRequest
    {
        public int? ProductId { get; set; }
        public int? ProductTuitionId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public DateTime? ArrivalDate { get; set; }
    }
}