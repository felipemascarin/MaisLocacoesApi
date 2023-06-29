using MaisLocacoes.WebApi.Context;
using MaisLocacoes.WebApi.Repository.v1.Entity.UserSchema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.v1.Entity.UserSchema
{
    [Table(TableNameEnum.Companies)]
    public class CompanyEntity
    {
        [Key]
        [StringLength(14)]
        [Column(TypeName = "char")]
        public string Cnpj { get; set; }

        public int CompanyAddressId { get; set; }

        public virtual CompanyAddressEntity CompanyAddressEntity { get; set; }

        [Required]
        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string CompanyName { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? StateRegister { get; set; }

        [Required]
        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string FantasyName { get; set; }

        [Required]
        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string Cel { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? Tel { get; set; }

        [Required]
        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        //Unique configurado na classe PostgreSqlContext
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string Segment { get; set; }

        [Required]
        [StringLength(2048)]
        [Column(TypeName = "character varying(2048)")]
        public string PjDocumentUrl { get; set; }

        [Required]
        [StringLength(2048)]
        [Column(TypeName = "character varying(2048)")]
        public string AddressDocumentUrl { get; set; }

        [StringLength(2048)]
        [Column(TypeName = "character varying(2048)")]
        public string? LogoUrl { get; set; }

        public int NotifyDaysBefore { get; set; }

        [Required]
        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string Status { get; set; }

        [Required]
        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string Module { get; set; }

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

        public ICollection<UserEntity> Users { get; set; }
    }
}