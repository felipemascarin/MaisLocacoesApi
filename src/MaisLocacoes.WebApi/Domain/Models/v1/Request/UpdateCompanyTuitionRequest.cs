namespace MaisLocacoes.WebApi.Domain.Models.v1.Request
{
    public class UpdateCompanyTuitionRequest
    {
        public int? AsaasNumber { get; set; }
        public int? TuitionNumber { get; set; }
        public decimal Value { get; set; }
        public DateTime? PayDate { get; set; }
        public DateTime DueDate { get; set; }
    }
}

