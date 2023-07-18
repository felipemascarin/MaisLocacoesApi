using MaisLocacoes.WebApi.Context;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.v1.Entity
{
    [Table(TableNameEnum.Oss)]
    public class OsEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ProductTuitionId { get; set; }
        public virtual ProductTuitionEntity ProductTuitionEntity { get; set; }

        [Required]
        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string Type { get; set; }

        [StringLength(11)]
        [Column(TypeName = "character varying(11)")]
        public string? DeliveryCpf { get; set; }

        [Required]
        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string Status { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? InitialDateTime { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? FinalDateTime { get; set; }

        [StringLength(1000)]
        [Column(TypeName = "character varying(1000)")]
        public string? DeliveryObservation { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "timestamp")]
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