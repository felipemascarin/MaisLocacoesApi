namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.Get
{
    public class GetSupplierByIdResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Cnpj { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Cel { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public CreateAddressResponse Address { get; set; }
    }
}