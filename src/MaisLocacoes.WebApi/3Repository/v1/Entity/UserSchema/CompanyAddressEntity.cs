using MaisLocacoes.WebApi.Context;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Repository.v1.Entity.UserSchema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaisLocacoes.WebApi.Repository.v1.Entity.UserSchema
{
    [Table(TableNameEnum.CompanyAddress)]
    public class CompanyAddressEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? Cep { get; set; }

        [StringLength(500)]
        [Column(TypeName = "character varying(500)")]
        public string? Street { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? Number { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? Complement { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? District { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? City { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? State { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string Country { get; set; }

        [Column(TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "timestamp without time zone")]
        public DateTime? UpdatedAt { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? CreatedBy { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? UpdatedBy { get; set; }

        public ICollection<CompanyEntity> Companies { get; set; }
    }
}