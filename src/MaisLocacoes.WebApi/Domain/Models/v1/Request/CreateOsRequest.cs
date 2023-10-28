namespace MaisLocacoes.WebApi.Domain.Models.v1.Request
{
    public class CreateOsRequest
    {
        public string Type { get; set; }
        public string DeliveryCpf { get; set; }
        public int ProductTuitionId { get; set; }
        public DateTime? InitialDateTime { get; set; }
        public DateTime? FinalDateTime { get; set; }
        public string DeliveryObservation { get; set; }
    }
}