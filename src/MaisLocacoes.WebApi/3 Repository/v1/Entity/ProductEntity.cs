using MaisLocacoes.WebApi.Context;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.v1.Entity
{
    [Table(TableNameEnum.Products)]
    public class ProductEntity
    {
        //[Key] Chave composta configurada na classe PostgreSqlContext
        [Required]
        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string Code { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string ProductType { get; set; }

        public virtual ProductTypeEntity ProductTypeEntity { get; set; }

        public int? SupplierId { get; set; }

        [StringLength(1000)]
        [Column(TypeName = "character varying(1000)")]
        public string? Description { get; set; }

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

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? CreatedBy { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? UpdatedBy { get; set; }

        public ICollection<RentedPlaceEntity> RentedPlaces { get; set; }
    }
}