using MaisLocacoes.WebApi.Context;
using Repository.v1.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaisLocacoes.WebApi._3Repository.v1.Entity
{
    [Table(TableNameEnum.Contracts)]
    public class ContractEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "uuid")]
        public Guid GuidId { get; set; }

        public int RentId { get; set; }
        public virtual RentEntity RentEntity { get; set; }

        public int? ProductQuantity { get; set; }

        [StringLength(2048)]
        [Column(TypeName = "character varying(2048)")]
        public string? UrlSignature { get; set; }

        [Column(TypeName = "timestamp without time zone")]
        public DateTime? SignedAt { get; set; }

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

        public bool? Deleted { get; set; }
    }
}
