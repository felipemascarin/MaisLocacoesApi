namespace MaisLocacoes.WebApi.Domain.Models.v1.Request
{
    public class UpdateQgRequest
    {
        public string Description { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public virtual UpdateAddressRequest Address { get; set; }
    }
}