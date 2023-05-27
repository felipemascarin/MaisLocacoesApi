namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.Get
{
    public class GetClientForRentResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DocumentNumber { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class GetClientForRentDtoResponse
    {
        public int Id { get; set; }
        public string Cpf { get; set; }
        public string ClientName { get; set; }
        public string Cnpj { get; set; }
        public string FantasyName { get; set; }
    }
}