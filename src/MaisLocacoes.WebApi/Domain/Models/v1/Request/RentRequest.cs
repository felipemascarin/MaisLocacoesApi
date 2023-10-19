using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Request
{
    public class RentRequest
    {
        public int ClientId { get; set; }
        public decimal? Carriage { get; set; }
        public string? Description { get; set; }
        public DateTime? SignedAt { get; set; }
        public string? UrlSignature { get; set; }
        public virtual AddressRequest Address { get; set; }
    }
}