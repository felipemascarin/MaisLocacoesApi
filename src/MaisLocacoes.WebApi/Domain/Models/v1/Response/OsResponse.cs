namespace MaisLocacoes.WebApi.Domain.Models.v1.Response
{
    public class OsResponse
    {
        public int Id { get; set; }
        public string DeliveryCpf { get; set; }
        public int RentId { get; set; }
        public string Status { get; set; }
        public DateTime? InitialDateTime { get; set; }
        public DateTime? FinalDateTime { get; set; }
        public string Description { get; set; }
        public string DeliveryObservation { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}