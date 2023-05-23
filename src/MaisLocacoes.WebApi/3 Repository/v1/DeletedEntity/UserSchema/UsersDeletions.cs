using MaisLocacoes.WebApi.Context;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaisLocacoes.WebApi._3_Repository.v1.DeletedEntity.UserSchema
{
    [Table(nameof(TableNameEnum.UsersDeletions))]
    public class UsersDeletions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PeopleDeletionsId { get; set; }

        [StringLength(11)]
        [Column(TypeName = "char")]
        public string? Cpf { get; set; }

        [StringLength(14)]
        [Column(TypeName = "char")]
        public string? Cnpj { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? Rg { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? Name { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? Email { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? Role { get; set; }

        [StringLength(2048)]
        [Column(TypeName = "character varying(2048)")]
        public string? ProfileImageUrl { get; set; }

        public DateTime? BornDate { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? Cel { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? CivilStatus { get; set; }

        [StringLength(2048)]
        [Column(TypeName = "character varying(2048)")]
        public string? CpfDocumentUrl { get; set; }

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
