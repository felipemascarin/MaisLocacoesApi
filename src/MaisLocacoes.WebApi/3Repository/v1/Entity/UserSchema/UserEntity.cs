using MaisLocacoes.WebApi.Context;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.v1.Entity.UserSchema
{
    [Table(TableNameEnum.Users)]
    public class UserEntity
    {
        [Key]
        [StringLength(11)]
        [Column(TypeName = "char")]
        public string Cpf { get; set; }

        [StringLength(14)]
        [Column(TypeName = "char")]
        public string Cnpj { get; set; }
        public string Cnpjs { get; set; }

        public virtual CompanyEntity CompanyEntity { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string Rg { get; set; }

        [Required]
        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        //Unique configurado na classe na classe PostgreSqlContext
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string Role { get; set; }

        [StringLength(2048)]
        [Column(TypeName = "character varying(2048)")]
        public string ProfileImageUrl { get; set; }

        [Column(TypeName = "date")]
        public DateTime? BornDate { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string Cel { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string CivilStatus { get; set; }

        [StringLength(2048)]
        [Column(TypeName = "character varying(2048)")]
        public string CpfDocumentUrl { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string Status { get; set; }

        [StringLength(3000)]
        [Column(TypeName = "character varying(3000)")]
        public string LastToken { get; set; }

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
    }
}