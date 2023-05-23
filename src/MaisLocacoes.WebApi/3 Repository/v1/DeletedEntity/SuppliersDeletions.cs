using MaisLocacoes.WebApi.Context;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaisLocacoes.WebApi._3_Repository.v1.DeletedEntity
{
    [Table(nameof(TableNameEnum.SuppliersDeletions))]
    public class SuppliersDeletions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SuppliersDeletionsId { get; set; }

        public int? AddressId { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? Name { get; set; }

        [StringLength(14)]
        [Column(TypeName = "char")]
        public string? Cnpj { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? Email { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? Tel { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? Cel { get; set; }

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