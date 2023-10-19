namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.Get
{
    public class GetAllProductTuitionByProductIdResponse
    {
        public List<ResumedRentDto> ProductTuitionsRentResponse { get; set; }
        public decimal TotalBilledValue { get; set; }

        public class ResumedRentDto
        {
            public int Id { get; set; }
            public DateTime InitialDateTime { get; set; }
            public DateTime FinalDateTime { get; set; }
            public decimal BilledValue { get; set; }
        }
    }
}
