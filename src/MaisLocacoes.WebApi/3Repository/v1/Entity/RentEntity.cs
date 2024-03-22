using MaisLocacoes.WebApi._3Repository.v1.Entity;
using MaisLocacoes.WebApi.Context;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.v1.Entity
{
    [Table(TableNameEnum.Rents)]
    public class RentEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ClientId { get; set; }
        public virtual ClientEntity Client { get; set; }

        public int AddressId { get; set; }
        public virtual AddressEntity Address { get; set; }

        [Required]
        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string Status { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Carriage { get; set; }

        [StringLength(1000)]
        [Column(TypeName = "character varying(1000)")]
        public string Description { get; set; }

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

        public ICollection<BillEntity> Bills { get; set; }
        public ICollection<ProductTuitionEntity> ProductTuitions { get; set; }
        public ICollection<ContractEntity> Contracts { get; set; }
    }
}