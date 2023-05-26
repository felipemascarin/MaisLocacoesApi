using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Repository.v1.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.Get
{
    public class GetBillResponse
    {
        public int Id { get; set; }
        public int RentId { get; set; }
        public decimal Value { get; set; }
        public DateTime? PayDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
        public int? NfIdFireBase { get; set; }
        public string? PaymentMode { get; set; }
    }
}