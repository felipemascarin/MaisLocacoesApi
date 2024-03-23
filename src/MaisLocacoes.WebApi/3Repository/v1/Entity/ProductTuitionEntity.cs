using MaisLocacoes.WebApi.DataBase.Context.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.v1.Entity
{
    [Table(TableNameEnum.ProductTuitions)]
    public class ProductTuitionEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int RentId { get; set; }
        public virtual RentEntity Rent { get; set; }

        public int ProductTypeId { get; set; }
        public virtual ProductTypeEntity ProductType { get; set; }

        public int? ProductId { get; set; }
        public virtual ProductEntity Product { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string ProductCode { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Value { get; set; }

        [Required]
        [Column(TypeName = "timestamp without time zone")]
        public DateTime InitialDateTime { get; set; }

        [Required]
        [Column(TypeName = "timestamp without time zone")]
        public DateTime FinalDateTime { get; set; }

        [DefaultValue(1)]
        public int Parts { get; set; }

        [Required]
        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string Status { get; set; }

        [Column(TypeName = "timestamp without time zone")]
        public DateTime? FirstDueDate { get; set; }

        public int QuantityPeriod { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string TimePeriod { get; set; }

        public bool? IsEditable { get; set; }

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

        public ICollection<OsEntity> Oss { get; set; }
        public ICollection<BillEntity> Bills { get; set; }
        public ICollection<RentedPlaceEntity> RentedPlaces { get; set; }
    }
}