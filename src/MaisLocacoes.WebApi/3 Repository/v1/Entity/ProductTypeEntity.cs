using MaisLocacoes.WebApi.Context;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.v1.Entity
{
    [Table(TableNameEnum.ProductTypes)]
    public class ProductTypeEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string Type { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? CreatedBy { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? UpdatedBy { get; set; }

        public string? UpdatedByanderson{ get; set; }

        public bool IsManyParts { get; set; }

        public bool? Deleted { get; set; }

        public ICollection<ProductEntity> Products { get; set; }
        public ICollection<ProductTuitionEntity> ProductTuitions { get; set; }
    }
}