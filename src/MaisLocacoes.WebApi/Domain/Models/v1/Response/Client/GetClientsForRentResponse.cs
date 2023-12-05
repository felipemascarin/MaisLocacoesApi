namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.Get
{
    public class GetClientsForRentResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DocumentNumber { get; set; }
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