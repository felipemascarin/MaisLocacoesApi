using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Validator
{
    public class RentedPlaceValidator : AbstractValidator<RentedPlaceRequest>
    {
        public RentedPlaceValidator()
        {
            RuleFor(rentedPlace => rentedPlace.ProductId)
               .Must(productId => int.TryParse(productId.ToString(), out var result) &&
                result > 0)
               .WithMessage("Id do tipo de produto deve ser um número inteiro maior que 0");

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

            RuleFor(qg => qg.Address).SetValidator(new AddressValidator());
        }
    }
}