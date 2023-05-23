namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.Create
{
    public class CreateAddressResponse
    {
        public int Id { get; set; }
        public string? Cep { get; set; }
        public string? Street { get; set; }
        public string? Number { get; set; }
        public string? Complement { get; set; }
        public string? District { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string Country { get; set; }
    }
}