namespace MaisLocacoes.WebApi.Domain.Models.v1.Request
{
    public class QgRequest
    {        
        public string Description { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public virtual AddressRequest Address { get; set; }
    }
}