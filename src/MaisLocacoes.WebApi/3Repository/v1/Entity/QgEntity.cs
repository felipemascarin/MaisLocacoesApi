using MaisLocacoes.WebApi.Context;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.v1.Entity
{
    [Table(TableNameEnum.Qgs)]
    public class QgEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int AddressId { get; set; }

        public virtual AddressEntity AddressEntity { get; set; }

        [Required]
        [StringLength(1000)]
        [Column(TypeName = "character varying(1000)")]
        public string Description { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

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