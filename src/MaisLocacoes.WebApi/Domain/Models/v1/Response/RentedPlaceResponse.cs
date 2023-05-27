using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Repository.v1.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Response
{
    public class RentedPlaceResponse
    {
        public int Id { get; set; }
        public string ProductType { get; set; }
        public string ProductCode { get; set; }
        public int AddressId { get; set; }
        public int RentId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}