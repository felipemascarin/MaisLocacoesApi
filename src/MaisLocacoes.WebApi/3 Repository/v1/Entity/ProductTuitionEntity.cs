using MaisLocacoes.WebApi.Context;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.v1.Entity
{
    [Table(TableNameEnum.ProductTuitions)]
    public class ProductTuitionEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int RentId { get; set; }
        public virtual RentEntity RentEntity { get; set; }

        public int ProductTypeId { get; set; }
        public virtual ProductTypeEntity ProductTypeEntity { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? ProductCode { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Value { get; set; }

        [Required]
        public DateTime InitialDateTime { get; set; }

        [Required]
        public DateTime FinalDateTime { get; set; }

        [DefaultValue(1)]
        public int Parts { get; set; }

        public DateTime CreatedAt { get; set; }

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