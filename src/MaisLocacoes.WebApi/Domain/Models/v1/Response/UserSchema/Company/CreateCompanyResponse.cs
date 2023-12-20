namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.UserSchema.Company
{
    public class CreateCompanyResponse
    {
        public string Cnpj { get; set; }
        public int AddressId { get; set; }
        public string CompanyName { get; set; }
        public string StateRegister { get; set; }
        public string FantasyName { get; set; }
        public string Cel { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public string Segment { get; set; }
        public string PjDocumentUrl { get; set; }
        public string AddressDocumentUrl { get; set; }
        public string LogoUrl { get; set; }
        public int NotifyDaysBefore { get; set; }
        public string Module { get; set; }
        public string TimeZone { get; set; }
        public string DataBase { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public CompanyAddressResponse CompanyAddress { get; set; }

        public class CompanyAddressResponse
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