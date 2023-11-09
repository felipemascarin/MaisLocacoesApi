namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.Contract
{
    public class GetContractInfoByRentIdResponse
    {
        public int Id { get; set; }
        public Guid GuidId { get; set; }
        public ContractRent Rent { get; set; }
        public int? ProductQuantity { get; set; }
        public string UrlSignature { get; set; }
        public DateTime SignedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public IEnumerable<ContractProductTuition> ProductTuitions { get; set; }

        public class ContractRent
        {
            public int Id { get; set; }
            public ContractClient Client { get; set; }
            public string Status { get; set; }
            public decimal? Carriage { get; set; }
            public string Description { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedAt { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime? UpdatedAt { get; set; }
            public DateTime? InitialRentDate { get; set; }
            public DateTime? FinalRentDate { get; set; }
            public ContractAddress Address { get; set; }
        }

        public class ContractProductTuition
        {
            public int Id { get; set; }
            public string ProductCode { get; set; }
            public int ProductTypeId { get; set; }
            public decimal Value { get; set; }
            public DateTime InitialDateTime { get; set; }
            public DateTime FinalDateTime { get; set; }
            public string Status { get; set; }
            public DateTime? FirstDueDate { get; set; }
            public int QuantityPeriod { get; set; }
            public string TimePeriod { get; set; }
            public int Parts { get; set; }
            public bool IsEditable { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedAt { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime? UpdatedAt { get; set; }
            public ContractProduct Product { get; set; }
        }

        public class ContractProduct
        {
            public int Id { get; set; }
            public string Code { get; set; }
            public int? SupplierId { get; set; }
            public string Description { get; set; }
            public DateTime? DateBought { get; set; }
            public decimal? BoughtValue { get; set; }
            public string Status { get; set; }
            public int? CurrentRentedPlaceId { get; set; }
            public int Parts { get; set; }
            public int RentedParts { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedAt { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime? UpdatedAt { get; set; }
            public ContractProductType ProductType { get; set; }
        }

        public class ContractProductType
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

        public class ContractClient
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
            public ContractAddress Address { get; set; }
        }

        public class ContractAddress
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
