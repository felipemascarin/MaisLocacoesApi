using MaisLocacoes.WebApi.DataBase.Context.Helpers;
using Repository.v1.Entity.UserSchema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaisLocacoes.WebApi._3Repository.v1.Entity.UserSchema
{
    [Table(TableNameEnum.CompaniesUsers)]
    public class CompanyUserEntity
    {
        [Required]
        [StringLength(14)]
        [Column(TypeName = "char")]
        public string Cnpj { get; set; }
        public virtual CompanyEntity Company { get; set; }

        [Required]
        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string Email { get; set; }
        public virtual UserEntity User { get; set; }
    }
}
