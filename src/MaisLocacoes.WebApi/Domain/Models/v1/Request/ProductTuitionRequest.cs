using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Repository.v1.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Request
{
    public class ProductTuitionRequest
    {
        public int RentId { get; set; }
        public int ProductTypeId { get; set; }
        public string? ProductCode { get; set; }
        public decimal Value { get; set; }
        public DateTime InitialDateTime { get; set; }
        public DateTime FinalDateTime { get; set; }
        public int Parts { get; set; }
    }
}