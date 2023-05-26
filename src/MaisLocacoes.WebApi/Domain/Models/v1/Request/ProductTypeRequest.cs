using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Request
{
    public class ProductTypeRequest
    {
        public string Type { get; set; }
        public bool IsManyParts { get; set; }
    }
}