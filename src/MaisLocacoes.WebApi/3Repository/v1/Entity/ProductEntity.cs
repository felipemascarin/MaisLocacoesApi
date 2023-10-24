using MaisLocacoes.WebApi.Context;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.v1.Entity
{
    [Table(TableNameEnum.Products)]
    public class ProductEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string Code { get; set; }

        public int ProductTypeId { get; set; }
        public virtual ProductTypeEntity ProductTypeEntity { get; set; }

        public int? SupplierId { get; set; }

        [StringLength(1000)]
        [Column(TypeName = "character varying(1000)")]
        public string? Description { get; set; }

        [Column(TypeName = "timestamp without time zone")]
        public DateTime? DateBought { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? BoughtValue { get; set; }

        [Required]
        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string Status { get; set; }

        public int? CurrentRentedPlaceId { get; set; }

        public int Parts { get; set; }

        public int RentedParts { get; set; }

        [Column(TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "timestamp without time zone")]
        public DateTime? UpdatedAt { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? CreatedBy { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? UpdatedBy { get; set; }

        public bool? Deleted { get; set; }

        public ICollection<RentedPlaceEntity> RentedPlaces { get; set; }
        public ICollection<ProductWasteEntity> ProductWastes { get; set; }
    }
}