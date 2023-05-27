using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Repository.v1.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Request
{
    public class RentRequest
    {
        public int ClientId { get; set; }
        public decimal? Carriage { get; set; }
        public string? Status { get; set; }
        public DateTime? FirstDueDate { get; set; }
        public virtual AddressRequest Address { get; set; }
    }
}