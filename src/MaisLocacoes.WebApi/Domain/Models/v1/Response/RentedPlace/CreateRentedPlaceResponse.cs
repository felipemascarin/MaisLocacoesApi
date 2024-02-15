namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.RentedPlace
{
    public class CreateRentedPlaceResponse
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int? ProductTuitionId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}