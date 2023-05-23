namespace MaisLocacoes.WebApi.Domain.Models.v1.Request
{
    public class RentedPlaceRequest
    {
        public string ProductType { get; set; }
        public string ProductCode { get; set; }
        public int RentId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public virtual AddressRequest Address { get; set; }
    }
}