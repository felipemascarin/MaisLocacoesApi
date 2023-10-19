namespace MaisLocacoes.WebApi.Domain.Models.v1.Request.Custom
{
    public class FinishOsRequest
    {
        public int ProductTuitionId { get; set; }
        public string ProductCode { get; set; }
        public int? QgId { get; set; }
        public string? DeliveryObservation { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}