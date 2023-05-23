using MaisLocacoes.WebApi.Context;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaisLocacoes.WebApi._3_Repository.v1.DeletedEntity
{
    [Table(nameof(TableNameEnum.AddressesDeletions))]
    public class AddressesDeletions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddressesDeletionsId { get; set; }

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
        public string? Country { get; set; }

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
