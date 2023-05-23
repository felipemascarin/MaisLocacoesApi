using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Validator
{
    public class RentedPlaceValidator : AbstractValidator<RentedPlaceRequest>
    {
        public RentedPlaceValidator()
        {
            RuleFor(rentedPlace => rentedPlace.ProductType)
                .Must(productType => !string.IsNullOrEmpty(productType))
                .WithMessage("O Tipo de produto é obrigatório")
                .MaximumLength(255)
                .WithMessage("O Tipo de produto ultrapassou o limite máximo de caracteres");

            RuleFor(rentedPlace => rentedPlace.ProductCode)
                .Must(productCode => !string.IsNullOrEmpty(productCode))
                .WithMessage("O Código do produto é obrigatório")
                .MaximumLength(255)
                .WithMessage("O Código do produto ultrapassou o limite máximo de caracteres");

            RuleFor(rentedPlace => rentedPlace.RentId)
               .Must(rentId => int.TryParse(rentId.ToString(), out var result) &&
                result > 0)
               .WithMessage("Id da locação deve ser um número inteiro maior que 0");

            RuleFor(qg => qg.Latitude)
                .Must(latitude => double.TryParse(latitude.ToString(), out var result))
                .WithMessage("Valor de Latitude se inserido deve ser válido")
                .When(qg => qg.Latitude != null);

            RuleFor(qg => qg.Longitude)
                .Must(longitude => double.TryParse(longitude.ToString(), out var result))
                .WithMessage("Valor de Longitude se inserido deve ser válido ")
                .When(qg => qg.Longitude != null);

            RuleFor(qg => qg.ArrivalDate)
                .Must(arrivalDate => DateTime.TryParse(arrivalDate.ToString(), out var result))
                .WithMessage("A Data de partida se inserida deve ser uma data válida")
                .When(client => client.ArrivalDate != null);

            //Chama o validator do Address
            RuleFor(qg => qg.Address).SetValidator(new AddressValidator());
        }
    }
}