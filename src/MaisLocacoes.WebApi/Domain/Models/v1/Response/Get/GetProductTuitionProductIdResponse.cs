namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.Get
{
    public class GetProductTuitionProductIdResponse
    {
        public List<GetProductTuitionRentResponse> ProductTuitionsRentResponse { get; set; }
        public decimal TotalBilledValue { get; set; }
    }
}
