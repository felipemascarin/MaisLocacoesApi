using MaisLocacoes.WebApi.Context;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.v1.Entity
{
    [Table(TableNameEnum.Addresses)]
    public class AddressEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

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
        public string Country { get; set; }

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

        public ICollection<ClientEntity> Clients { get; set; }
        public ICollection<QgEntity> Qgs { get; set; }
        public ICollection<RentEntity> Rents { get; set; }
        public ICollection<SupplierEntity> Suppliers { get; set; }
    }
}