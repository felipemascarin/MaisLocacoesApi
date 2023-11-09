namespace MaisLocacoes.WebApi.Domain.Models.v1.Response.ProductTuition
{
    public class GetProductTuitionByIdResponse
    {
        public int Id { get; set; }
        public RentResponse Rent { get; set; }
        public int ProductTypeId { get; set; }
        public int? ProductId { get; set; }
        public string ProductCode { get; set; }
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

        public class RentResponse
        {
            public int Id { get; set; }
            public int ClientId { get; set; }
            public string Status { get; set; }
            public decimal? Carriage { get; set; }
            public string Description { get; set; }
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
    }
}