using MaisLocacoes.WebApi.Context;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaisLocacoes.WebApi._3_Repository.v1.DeletedEntity
{
    [Table(nameof(TableNameEnum.ProductWastesDeletions))]
    public class ProductWastesDeletions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductWastesDeletionsId { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? Code { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? ProductType { get; set; }

        [StringLength(1000)]
        [Column(TypeName = "character varying(1000)")]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Value { get; set; }

        public DateTime? Date { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? CreatedBy { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? UpdatedBy { get; set; }

        public DateTime DeletedAt { get; set; }

        public string? DeletedBy { get; set; }
    }
}
