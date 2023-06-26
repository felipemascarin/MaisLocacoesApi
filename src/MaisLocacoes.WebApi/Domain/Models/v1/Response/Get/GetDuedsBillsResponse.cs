namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.Get
{
    public class GetDuedsBillsResponse
    {
        public string ClientName { get; set; }
        public DateTime DueDate { get; set; }
        public decimal? Value { get; set; }
        public string? ClientPhone { get; set; }
        public int RentId { get; set; }
        public int BillId { get; set; }
        public string? BillDescription { get; set;}
    }
}