using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Repository.v1.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Request
{
    public class OsRequest
    {
        public string? DeliveryCpf { get; set; }
        public int RentId { get; set; }
        public DateTime? InitialDateTime { get; set; }
        public DateTime? FinalDateTime { get; set; }
        public string? Description { get; set; }
        public string? DeliveryObservation { get; set; }
    }
}