using MaisLocacoes.WebApi.DataBase.Context.Helpers;
using Repository.v1.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaisLocacoes.WebApi._3Repository.v1.Entity
{
    [Table(TableNameEnum.OsPictures)]
    public class OsPictureEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? OsId { get; set; }
        public virtual OsEntity Os { get; set; }

        [StringLength(2048)]
        [Column(TypeName = "character varying(2048)")]
        public string PictureUrl { get; set; }
    }
}
