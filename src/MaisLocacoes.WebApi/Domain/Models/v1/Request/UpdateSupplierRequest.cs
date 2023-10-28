namespace MaisLocacoes.WebApi.Domain.Models.v1.Request
{
    public class UpdateSupplierRequest
    {
        public string Name { get; set; }
        public string Cnpj { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Cel { get; set; }
        public virtual UpdateAddressRequest Address { get; set; }
    }
}