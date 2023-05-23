using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Repository.v1.Entity;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Request
{
    public class SupplierRequest
    {
        public string Name { get; set; }
        public string? Cnpj { get; set; }
        public string? Email { get; set; }
        public string? Tel { get; set; }
        public string? Cel { get; set; }
        public virtual AddressRequest Address { get; set; }
    }
}