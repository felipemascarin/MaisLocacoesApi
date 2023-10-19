namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.Get
{
    public class GetOsByIdResponse
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string DeliveryCpf { get; set; }
        public int ProductTuitionId { get; set; }
        public string Status { get; set; }
        public DateTime? InitialDateTime { get; set; }
        public DateTime? FinalDateTime { get; set; }
        public string DeliveryObservation { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}