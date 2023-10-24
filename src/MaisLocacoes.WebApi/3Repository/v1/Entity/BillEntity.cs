using MaisLocacoes.WebApi.Context;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.v1.Entity
{
    [Table(TableNameEnum.Bills)]
    public class BillEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int RentId { get; set; }
        public virtual RentEntity RentEntity { get; set; }

        public int? ProductTuitionId { get; set; }
        public virtual ProductTuitionEntity ProductTuitionEntity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Value { get; set; }

        [Column(TypeName = "timestamp without time zone")]
        public DateTime? PayDate { get; set; }

        [Column(TypeName = "timestamp without time zone")]
        [Required]
        public DateTime DueDate { get; set; }
        
        [Column(TypeName = "timestamp without time zone")]
        public DateTime? InvoiceEmittedDate { get; set; }

        [Required]
        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string Status { get; set; }

        public int? NfIdFireBase { get; set; }

        public int? Order { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? PaymentMode { get; set; }

        [StringLength(1000)]
        [Column(TypeName = "character varying(1000)")]
        public string? Description { get; set; }

        [Column(TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "timestamp without time zone")]
        public DateTime? UpdatedAt { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? CreatedBy { get; set; }

        [StringLength(255)]
        [Column(TypeName = "character varying(255)")]
        public string? UpdatedBy { get; set; }

        public bool? Deleted { get; set; }
    }
}