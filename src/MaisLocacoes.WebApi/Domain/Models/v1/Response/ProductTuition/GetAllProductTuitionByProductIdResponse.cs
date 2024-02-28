namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.ProductTuition
{
    public class GetAllProductTuitionByProductIdResponse
    {
        public List<ResumedProductRentDto> ProductTuitionsRentResponse { get; set; }
        public decimal TotalBilledValue { get; set; }

        public class ResumedProductRentDto
        {
            public int Id { get; set; }
            public DateTime InitialDateTime { get; set; }
            public DateTime FinalDateTime { get; set; }
            public decimal BilledValue { get; set; }
        }
    }
}
