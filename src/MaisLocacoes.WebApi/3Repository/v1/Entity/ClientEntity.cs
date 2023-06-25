using MaisLocacoes.WebApi.Context;
using NpgsqlTypes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.v1.Entity
{
    [Table(TableNameEnum.Clients)]
    public class ClientEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string Type { get; set; }

        public int AddressId { get; set; }

        public virtual AddressEntity AddressEntity { get; set; }

        [StringLength(11)]
        [Column(TypeName = "character varying(11)")]
        public string? Cpf { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? Rg { get; set; }

        [StringLength(14)]
        [Column(TypeName = "character varying(14)")]
        public string? Cnpj { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? CompanyName { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? ClientName { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? StateRegister { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? FantasyName { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? Cel { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? Tel { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? Email { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? BornDate { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? Career { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? CivilStatus { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? Segment { get; set; }

        [StringLength(2048)]
        [Column(TypeName = "character varying(2048)")]
        public string? CpfDocumentUrl { get; set; }

        [StringLength(2048)]
        [Column(TypeName = "character varying(2048)")]
        public string? CnpjDocumentUrl { get; set; }

        [StringLength(2048)]
        [Column(TypeName = "character varying(2048)")]
        public string? AddressDocumentUrl { get; set; }

        [StringLength(2048)]
        [Column(TypeName = "character varying(2048)")]
        public string? ClientPictureUrl { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string Status { get; set; }

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

        public ICollection<RentEntity> Rents { get; set; }
    }
}