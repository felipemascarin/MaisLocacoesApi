using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Repository.v1.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Request
{
    public class BillRequest
    {
        public int RentId { get; set; }
        public decimal Value { get; set; }
        public DateTime? PayDate { get; set; }
        public DateTime DueDate { get; set; }
        public int? NfIdFireBase { get; set; }
        public string? PaymentMode { get; set; }
    }
}