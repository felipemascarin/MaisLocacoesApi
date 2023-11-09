using MaisLocacoes.WebApi.Domain.Models.v1.Response.Address;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.Client
{
    public class GetClientByCpfResponse
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public string Cnpj { get; set; }
        public string CompanyName { get; set; }
        public string ClientName { get; set; }
        public string StateRegister { get; set; }
        public string FantasyName { get; set; }
        public string Cel { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public DateTime? BornDate { get; set; }
        public string Career { get; set; }
        public string CivilStatus { get; set; }
        public string Segment { get; set; }
        public string CpfDocumentUrl { get; set; }
        public string CnpjDocumentUrl { get; set; }
        public string AddressDocumentUrl { get; set; }
        public string ClientPictureUrl { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Deleted { get; set; }
        public AddressResponse Address { get; set; }

        public class AddressResponse
        {
            public int Id { get; set; }
            public string Cep { get; set; }
            public string Street { get; set; }
            public string Number { get; set; }
            public string Complement { get; set; }
            public string District { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Country { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedAt { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime? UpdatedAt { get; set; }
        }
    }
}