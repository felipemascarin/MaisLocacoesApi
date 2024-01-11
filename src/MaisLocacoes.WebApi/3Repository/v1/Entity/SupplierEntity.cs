using MaisLocacoes.WebApi.Context;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.v1.Entity
{
    [Table(TableNameEnum.Suppliers)]
    public class SupplierEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int AddressId { get; set; }
        public virtual AddressEntity Address { get; set; }

        [Required]
        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string Name { get; set; }


        [StringLength(14)]
        [Column(TypeName = "character varying(14)")]
        public string Cnpj { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string Email { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string Tel { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string Cel { get; set; }

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

        public bool? Deleted { get; set; }
    }
}