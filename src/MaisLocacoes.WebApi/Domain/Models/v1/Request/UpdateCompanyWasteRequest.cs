namespace MaisLocacoes.WebApi.Domain.Models.v1.Request
{
    public class UpdateCompanyWasteRequest
    {
        public string Description { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
    }
}