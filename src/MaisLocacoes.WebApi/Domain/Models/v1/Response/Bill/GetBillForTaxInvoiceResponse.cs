namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.Bill
{
    public class GetBillForTaxInvoiceResponse
    {
        public int Id { get; set; }
        public int? ProductTuitionId { get; set; }
        public string ProductCode { get; set; }
        public int? ProductTuitionParts { get; set; }
        public decimal Value { get; set; }
        public DateTime? PayDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? InvoiceEmittedDate { get; set; }
        public string Status { get; set; }
        public int? InvoiceId { get; set; }
        public int? Order { get; set; }
        public string PaymentMode { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public RentResponse Rent { get; set; }
        public ProductTypeResponse ProductType { get; set; }
        public CompanyResponse Company { get; set; }

        public class RentResponse
        {
            public int Id { get; set; }
            public ClientResponse Client { get; set; }
            public string Status { get; set; }
            public decimal? Carriage { get; set; }
            public string Description { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedAt { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime? UpdatedAt { get; set; }
            public AddressResponse Address { get; set; }
        }

        public class ClientResponse
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
            public AddressResponse Address { get; set; }
        }

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

        public class ProductTypeResponse
        {
            public int Id { get; set; }
            public string Type { get; set; }
            public bool IsManyParts { get; set; }
            public string CreatedBy { get; set; }
            public string LastCreatedCode { get; set; }
            public DateTime CreatedAt { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime? UpdatedAt { get; set; }
        }

        public class CompanyResponse
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
            public string Status { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedAt { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime? UpdatedAt { get; set; }
            public CompanyAddressResponse CompanyAddress { get; set; }
        }

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