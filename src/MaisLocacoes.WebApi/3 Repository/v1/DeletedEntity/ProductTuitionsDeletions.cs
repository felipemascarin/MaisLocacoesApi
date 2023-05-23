using MaisLocacoes.WebApi.Context;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaisLocacoes.WebApi._3_Repository.v1.DeletedEntity
{
    [Table(nameof(TableNameEnum.ProductTuitionsDeletions))]
    public class ProductTuitionsDeletions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductTuitionsId { get; set; }

        public int? RentId { get; set; }

        public string? ProductType { get; set; }

        public string? ProductCode { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Value { get; set; }

        public DateTime? InitialDateTime { get; set; }

        public DateTime? FinalDateTime { get; set; }

        public int? Parts { get; set; }

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
