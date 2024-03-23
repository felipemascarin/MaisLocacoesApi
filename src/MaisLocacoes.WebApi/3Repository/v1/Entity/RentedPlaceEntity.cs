using MaisLocacoes.WebApi.DataBase.Context.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.v1.Entity
{
    [Table(TableNameEnum.RentedPlaces)]
    public class RentedPlaceEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? ProductId { get; set; }
        public virtual ProductEntity Product { get; set; }

        public int? ProductTuitionId { get; set; }
        public virtual ProductTuitionEntity ProductTuition { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [Column(TypeName = "timestamp without time zone")]
        public DateTime? ArrivalDate { get; set; }

        [Column(TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "timestamp without time zone")]
        public DateTime? UpdatedAt { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string CreatedBy { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string UpdatedBy { get; set; }

        public ICollection<QgEntity> Qgs { get; set; }
    }
}