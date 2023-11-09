namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.CompanyTuition
{
    public class GetCompanyTuitionByIdResponse
    {
        public int Id { get; set; }
        public int? AsaasNumber { get; set; }
        public int? TuitionNumber { get; set; }
        public decimal Value { get; set; }
        public DateTime? PayDate { get; set; }
        public DateTime DueDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}