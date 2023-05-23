using MaisLocacoes.WebApi.Context;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaisLocacoes.WebApi._3_Repository.v1.DeletedEntity
{
    [Table(nameof(TableNameEnum.RentedPlacesDeletions))]
    public class RentedPlacesDeletions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RentedPlacesDeletionsId { get; set; }

        public string? ProductType { get; set; }

        public string? ProductCode { get; set; }

        public int? AddressId { get; set; }

        public int? RentId { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public DateTime? ArrivalDate { get; set; }

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
